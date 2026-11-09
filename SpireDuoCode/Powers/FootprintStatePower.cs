using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Rooms;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Powers;

public enum Footprint
{
    Empty,
    Attack,
    Skill,
    Power
}

public enum FootprintLine
{
    Pounce,
    Groom,
    Omen
}

/// <summary>Optional combat listeners for effects which are driven by the Footprint state machine.</summary>
public interface IFootprintListener
{
    Task OnFootprintLine(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line) => Task.CompletedTask;
    Task OnFootprintSetOrRotated(PlayerChoiceContext context, CardModel? source) => Task.CompletedTask;
    Task OnCompanionResolved(PlayerChoiceContext context, CardPlay cardPlay, Creature? ally) => Task.CompletedTask;
    Task OnEmptySlotFilled(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
    Task OnFootprintCleared(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
}

/// <summary>Combat-local Black Cat state. A single hidden power records the owner's card plays.</summary>
public sealed class FootprintStatePower : SpireDuoPower
{
    private readonly Footprint[] _slots = [Footprint.Empty, Footprint.Empty, Footprint.Empty];
    private readonly bool[] _locked = [false, false, false];
    private int _omenTurn = -1;
    private Creature? _companionAlly;
    private CardType _companionType;
    private decimal _companionBlock;

    public IReadOnlyList<Footprint> Slots => _slots;
    public IReadOnlyList<bool> Locked => _locked;
    public bool CompanionArmed => _companionAlly != null;
    public bool HasTriggered(FootprintLine line) => _triggeredThisTurn.Contains(line);
    public int PouncesThisCombat { get; private set; }
    public int PlayedCardsThisTurn { get; private set; }
    private readonly HashSet<FootprintLine> _triggeredThisTurn = [];

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    protected override bool IsVisibleInternal => false;

    public static async Task<FootprintStatePower> GetOrCreate(PlayerChoiceContext context, Creature creature, CardModel? source = null)
    {
        var state = creature.GetPower<FootprintStatePower>();
        if (state != null)
            return state;

        await PowerCmd.Apply<FootprintStatePower>(context, creature, 1, creature, source);
        return creature.GetPower<FootprintStatePower>()!;
    }

    public static async Task Set(PlayerChoiceContext context, Creature creature, int index, Footprint footprint, CardModel? source = null)
    {
        var state = await GetOrCreate(context, creature, source);
        await state.Set(context, index, footprint, source);
    }

    public static async Task Lock(PlayerChoiceContext context, Creature creature, int index, CardModel? source = null)
    {
        var state = await GetOrCreate(context, creature, source);
        state._locked[index] = true;
    }

    public static async Task LockAllNonEmpty(PlayerChoiceContext context, Creature creature, CardModel? source = null)
    {
        var state = await GetOrCreate(context, creature, source);
        for (int i = 0; i < state._slots.Length; i++)
            if (state._slots[i] != Footprint.Empty)
                state._locked[i] = true;
    }

    public static async Task UnlockAll(PlayerChoiceContext context, Creature creature, CardModel? source = null)
    {
        var state = await GetOrCreate(context, creature, source);
        Array.Fill(state._locked, false);
    }

    public static async Task CopyNonEmptySlotsIntoAdjacentEmptySlots(PlayerChoiceContext context, Creature creature, CardModel? source = null)
    {
        var state = await GetOrCreate(context, creature, source);
        Footprint[] original = (Footprint[])state._slots.Clone();

        for (int index = 0; index < original.Length; index++)
        {
            if (original[index] == Footprint.Empty)
                continue;

            int right = index + 1;
            int left = index - 1;
            if (right < original.Length && state._slots[right] == Footprint.Empty && !state._locked[right])
                state._slots[right] = original[index];
            else if (left >= 0 && state._slots[left] == Footprint.Empty && !state._locked[left])
                state._slots[left] = original[index];
        }

        await state.NotifySetOrRotated(context, source);
    }

    public static async Task SetAll(PlayerChoiceContext context, Creature creature, Footprint first, Footprint second, Footprint third, CardModel? source = null)
    {
        var state = await GetOrCreate(context, creature, source);
        state._slots[0] = first;
        state._slots[1] = second;
        state._slots[2] = third;
        await state.NotifySetOrRotated(context, source);
    }

    public static async Task Clear(PlayerChoiceContext context, Creature creature, CardPlay cardPlay)
    {
        var state = await GetOrCreate(context, creature, cardPlay.Card);
        await state.Clear(context, cardPlay);
    }

    public async Task TriggerOmenNow(PlayerChoiceContext context, CardPlay cardPlay) => await TriggerOmen(context, cardPlay);

    public async Task TriggerLineNow(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line)
    {
        switch (line)
        {
            case FootprintLine.Pounce:
                await TriggerPounce(context, cardPlay);
                break;
            case FootprintLine.Groom:
                await TriggerGroom(context, cardPlay);
                break;
            case FootprintLine.Omen:
                await TriggerOmen(context, cardPlay);
                break;
        }
    }

    public async Task ResolveCompanionNow(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (!CompanionArmed)
            return;

        if (_companionAlly is { IsDead: false })
            await CreatureCmd.GainBlock(_companionAlly, _companionBlock, ValueProp.Move, cardPlay);
        await NotifyCompanionResolved(context, cardPlay, _companionAlly);
        DisarmCompanion();
    }

    public static async Task ArmCompanion(PlayerChoiceContext context, Creature creature, Creature? ally, CardType type, decimal block, CardModel? source = null)
    {
        var state = await GetOrCreate(context, creature, source);
        state._companionAlly = ally;
        state._companionType = type;
        state._companionBlock = block;
    }

    public async Task Set(PlayerChoiceContext context, int index, Footprint footprint, CardModel? source = null)
    {
        if (!_locked[index])
        {
            _slots[index] = footprint;
            await NotifySetOrRotated(context, source);
        }
    }

    public async Task RotateRight(PlayerChoiceContext context, CardModel? source = null)
    {
        Footprint slot = _slots[2];
        bool locked = _locked[2];
        _slots[2] = _slots[1];
        _locked[2] = _locked[1];
        _slots[1] = _slots[0];
        _locked[1] = _locked[0];
        _slots[0] = slot;
        _locked[0] = locked;
        await NotifySetOrRotated(context, source);
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay) => Record(choiceContext, cardPlay);

    public async Task Record(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || Owner.Player?.Character is not BlackCat)
            return;

        Footprint footprint = cardPlay.Card.Type switch
        {
            CardType.Attack => Footprint.Attack,
            CardType.Skill => Footprint.Skill,
            CardType.Power => Footprint.Power,
            _ => Footprint.Empty
        };
        if (footprint == Footprint.Empty)
            return;

        PlayedCardsThisTurn++;

        if (CompanionArmed && cardPlay.Card.Type == _companionType)
            await ResolveCompanionNow(choiceContext, cardPlay);

        bool filledEmpty = RecordIntoUnlockedSlots(footprint);
        if (filledEmpty)
            await NotifyEmptySlotFilled(choiceContext, cardPlay);

        if (_slots.SequenceEqual([Footprint.Attack, Footprint.Skill, Footprint.Attack]))
            await TriggerPounce(choiceContext, cardPlay);
        else if (_slots.SequenceEqual([Footprint.Skill, Footprint.Skill, Footprint.Attack]))
            await TriggerGroom(choiceContext, cardPlay);
        else if (_slots.SequenceEqual([Footprint.Power, Footprint.Skill, Footprint.Power]))
            await TriggerOmen(choiceContext, cardPlay);
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            Array.Fill(_locked, false);
            _triggeredThisTurn.Clear();
            PlayedCardsThisTurn = 0;
        }
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        DisarmCompanion();
        return Task.CompletedTask;
    }

    private bool RecordIntoUnlockedSlots(Footprint footprint)
    {
        int[] writable = Enumerable.Range(0, _slots.Length).Where(index => !_locked[index]).ToArray();
        if (writable.Length == 0)
            return false;

        bool filledEmpty = _slots[writable[^1]] == Footprint.Empty;

        for (int i = 0; i < writable.Length - 1; i++)
            _slots[writable[i]] = _slots[writable[i + 1]];
        _slots[writable[^1]] = footprint;
        return filledEmpty;
    }

    public bool WillTrigger(FootprintLine line, Footprint next)
    {
        int[] writable = Enumerable.Range(0, _slots.Length).Where(index => !_locked[index]).ToArray();
        if (writable.Length == 0)
            return false;

        Footprint[] predicted = (Footprint[])_slots.Clone();
        for (int i = 0; i < writable.Length - 1; i++)
            predicted[writable[i]] = predicted[writable[i + 1]];
        predicted[writable[^1]] = next;

        return line switch
        {
            FootprintLine.Pounce => predicted.SequenceEqual([Footprint.Attack, Footprint.Skill, Footprint.Attack]),
            FootprintLine.Groom => predicted.SequenceEqual([Footprint.Skill, Footprint.Skill, Footprint.Attack]),
            FootprintLine.Omen => predicted.SequenceEqual([Footprint.Power, Footprint.Skill, Footprint.Power]),
            _ => false
        };
    }

    private async Task TriggerPounce(PlayerChoiceContext context, CardPlay cardPlay)
    {
        var enemies = Owner.CombatState!.HittableEnemies;
        if (enemies.Count > 0)
        {
            var enemy = cardPlay.Target is { IsDead: false } target && enemies.Contains(target)
                ? target
                : enemies.MaxBy(enemy => enemy.CurrentHp)!;
            await DamageCmd.Attack(8).FromCard(cardPlay.Card).Targeting(enemy).WithHitFx("vfx/vfx_attack_slash").Execute(context);
            await PowerCmd.Apply<VulnerablePower>(context, enemy, 1, Owner, cardPlay.Card);
        }
        await NotifyLine(context, cardPlay, FootprintLine.Pounce);
        await Clear(context, cardPlay);
    }

    private async Task TriggerGroom(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner, 8, ValueProp.Unpowered, cardPlay);
        await CardPileCmd.Draw(context, 1, Owner.Player!);
        await NotifyLine(context, cardPlay, FootprintLine.Groom);
        await Clear(context, cardPlay);
    }

    private async Task TriggerOmen(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(1, Owner.Player!);
        var retained = (await CardSelectCmd.FromHand(
            prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1),
            context: context,
            player: Owner.Player!,
            filter: null,
            source: cardPlay.Card)).FirstOrDefault();
        retained?.GiveSingleTurnRetain();

        var velvet = Owner.GetPower<VelvetEclipsePower>();
        int turn = Owner.Player!.PlayerCombatState?.TurnNumber ?? -1;
        if (velvet != null && _omenTurn != turn)
        {
            _omenTurn = turn;
            await CardPileCmd.Draw(context, velvet.Amount, Owner.Player!);
        }
        await NotifyLine(context, cardPlay, FootprintLine.Omen);
        await Clear(context, cardPlay);
    }

    private async Task Clear(PlayerChoiceContext context, CardPlay cardPlay)
    {
        Array.Fill(_slots, Footprint.Empty);
        Array.Fill(_locked, false);
        Flash();
        foreach (IFootprintListener listener in Listeners().ToList())
            await listener.OnFootprintCleared(context, cardPlay);
    }

    private void DisarmCompanion()
    {
        _companionAlly = null;
        _companionType = CardType.None;
        _companionBlock = 0;
    }

    private async Task NotifyLine(PlayerChoiceContext context, CardPlay play, FootprintLine line)
    {
        _triggeredThisTurn.Add(line);
        if (line == FootprintLine.Pounce)
            PouncesThisCombat++;

        foreach (IFootprintListener listener in Listeners().ToList())
            await listener.OnFootprintLine(context, play, line);
    }

    private async Task NotifySetOrRotated(PlayerChoiceContext context, CardModel? source)
    {
        foreach (IFootprintListener listener in Listeners().ToList())
            await listener.OnFootprintSetOrRotated(context, source);
    }

    private async Task NotifyEmptySlotFilled(PlayerChoiceContext context, CardPlay play)
    {
        foreach (IFootprintListener listener in Listeners().ToList())
            await listener.OnEmptySlotFilled(context, play);
    }

    private async Task NotifyCompanionResolved(PlayerChoiceContext context, CardPlay play, Creature? ally)
    {
        foreach (IFootprintListener listener in Listeners().ToList())
            await listener.OnCompanionResolved(context, play, ally);
    }

    private IEnumerable<IFootprintListener> Listeners() =>
        Owner.Powers.OfType<IFootprintListener>().Concat(Owner.Player?.Relics.OfType<IFootprintListener>() ?? []);
}
