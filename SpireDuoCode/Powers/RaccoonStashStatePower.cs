using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Rooms;
using SpireDuo.SpireDuoCode.Progress;

namespace SpireDuo.SpireDuoCode.Powers;
/// <summary> Combat-local, replicated Stash ownership. Cards have no pile while held here.</summary>
public sealed class RaccoonStashStatePower : SpireDuoPower
{
    private readonly List<CardModel > _slots = [];
    private int _capacity = 3;
    private int _stashedTurn = - 1;
    public IReadOnlyList<CardModel> Slots => _slots;
    public int Capacity => _capacity;
    public int Count => _slots.Count;
    public bool SpentThisTurn
    {
        get;
        private set;
    }
    public bool StashedThisTurn => _stashedTurn == Owner.Player?.PlayerCombatState?.TurnNumber;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    protected override bool IsVisibleInternal => false;
    public static async Task<RaccoonStashStatePower> GetOrCreate(PlayerChoiceContext context, Creature creature, CardModel? source = null)
    {
        var state = creature.GetPower<RaccoonStashStatePower>();
        if (state != null)
            return state;
        await PowerCmd.Apply<RaccoonStashStatePower>(context, creature, 1, creature, source);
        return creature.GetPower<RaccoonStashStatePower>()!;
    }
    public static Player? DeterministicAlly(Creature owner)
    {
        return owner.Player?.RunState.Players.Where(player => player != owner.Player && player.Creature.IsAlive).OrderBy(player => owner.Player.RunState.GetPlayerSlotIndex(player)).FirstOrDefault();
    }
    public static int CostValue(CardModel card) => card.EnergyCost.CostsX? 3 : Math.Clamp(card.EnergyCost.GetWithModifiers(CostModifiers.Local), 0, 3);
    public void IncreaseCapacity(int amount)
    {
        _capacity += amount;
    }
    public static async Task < int> StashFromHand(PlayerChoiceContext context, Player player, int amount, CardModel source, Func<CardModel, bool>? filter = null, int minimum = 1)
    {
        var state = await GetOrCreate(context, player.Creature, source);
        int count = Math.Min(amount, state.Capacity - state.Count);
        if (count <= 0)
            return 0;
        if (!PileType.Hand.GetPile(player).Cards.Any(card =>!card.Keywords.Contains(CardKeyword.Ethereal) &&(filter?.Invoke(card) ?? true)))
            return 0;
        var cards = await CardSelectCmd.FromHand(context, player, new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER"), minimum, count), card =>!card.Keywords.Contains(CardKeyword.Ethereal) &&(filter?.Invoke(card) ?? true), source);
        int moved = 0;
        foreach (var card in cards.ToList()) if (await state.Stash(context, card, source)) moved++;
        return moved;
    }
    public static async Task < bool> StashTopDraw(PlayerChoiceContext context, Player player, CardModel source)
    {
        var state = await GetOrCreate(context, player.Creature, source);
        var card = PileType.Draw.GetPile(player).Cards.FirstOrDefault();
        return card != null && await state.Stash(context, card, source);
    }
    public static async Task < bool> StashFromTopDraw(PlayerChoiceContext context, Player player, int amount, CardModel source)
    {
        var state = await GetOrCreate(context, player.Creature, source);
        if (state.Count >= state.Capacity)
            return false;
        var candidates = PileType.Draw.GetPile(player).Cards.Take(amount).ToList();
        if (candidates.Count == 0)
            return false;
        var card =(await CardSelectCmd.FromSimpleGrid(context, candidates, player, new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER"), 1))).FirstOrDefault();
        if (card == null ||!await state.Stash(context, card, source))
            return false;
        var remaining = candidates.Where(candidate => candidate != card).ToList();
        var ordered = new List<CardModel>();
        while (remaining.Count > 0)
        {
            var next =(await CardSelectCmd.FromSimpleGrid(context, remaining, player, new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER"), 1))).FirstOrDefault();
            if (next == null)
                break;
            next.RemoveFromCurrentPile(true);
            ordered.Add(next);
            remaining.Remove(next);
        }
        foreach (var remainingCard in remaining) remainingCard.RemoveFromCurrentPile(true);
        foreach (var orderedCard in ordered.Concat(remaining).Reverse()) await CardPileCmd.Add(orderedCard, PileType.Draw, CardPilePosition.Top);
        return true;
    }
    public static async Task < bool> StashFromDiscard(PlayerChoiceContext context, Player player, CardModel source)
    {
        var state = await GetOrCreate(context, player.Creature, source);
        if (state.Count >= state.Capacity)
            return false;
        var candidates = PileType.Discard.GetPile(player).Cards.Where(card =>!card.Keywords.Contains(CardKeyword.Ethereal)).ToList();
        if (candidates.Count == 0)
            return false;
        var selected =(await CardSelectCmd.FromSimpleGrid(context, candidates, player, new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER"), 1))).FirstOrDefault();
        return selected != null && await state.Stash(context, selected, source);
    }
    public static async Task<CardModel? > Spend(PlayerChoiceContext context, Creature creature, CardModel source, Func<CardModel, bool>? filter = null)
    {
        var state = await GetOrCreate(context, creature, source);
        var candidates = state._slots.Where(card => filter?.Invoke(card) ?? true).ToList();
        if (candidates.Count == 0)
            return null;
        var card =(await CardSelectCmd.FromSimpleGrid(context, candidates, creature.Player!, new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER"), 1))).FirstOrDefault();
        return card == null? null : await state.Spend(context, card, source)? card : null;
    }
    public static async Task < int> SpendAll(PlayerChoiceContext context, Creature creature, CardModel source)
    {
        var state = await GetOrCreate(context, creature, source);
        int count = 0;
        foreach (var card in state._slots.ToList()) if (await state.Spend(context, card, source)) count++;
        return count;
    }
    public static async Task < int> SpendUpTo(PlayerChoiceContext context, Creature creature, CardModel source, int amount)
    {
        var state = await GetOrCreate(context, creature, source);
        if (state._slots.Count == 0)
            return 0;
        var cards = await CardSelectCmd.FromSimpleGrid(context, state._slots, creature.Player!, new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER"), 0, Math.Min(amount, state._slots.Count)));
        int spent = 0;
        foreach (var card in cards.ToList()) if (await state.Spend(context, card, source)) spent++;
        return spent;
    }
    public static async Task<CardModel? > Unstash(PlayerChoiceContext context, Creature creature, CardModel source)
    {
        var state = await GetOrCreate(context, creature, source);
        if (state._slots.Count == 0)
            return null;
        var card =(await CardSelectCmd.FromSimpleGrid(context, state._slots, creature.Player!, new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER"), 1))).FirstOrDefault();
        if (card == null ||!state._slots.Remove(card))
            return null;
        await CardPileCmd.Add(card, PileType.Hand);
        foreach (var listener in state.Listeners().ToList()) await listener.OnUnstashed(context, state, card, source);
        return card;
    }
    public static async Task<CardModel? > Choose(PlayerChoiceContext context, Creature creature)
    {
        var state = await GetOrCreate(context, creature);
        if (state._slots.Count == 0)
            return null;
        return(await CardSelectCmd.FromSimpleGrid(context, state._slots, creature.Player!, new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER"), 1))).FirstOrDefault();
    }
    public static async Task < int> StashRandomDiscard(PlayerChoiceContext context, Player player, int amount, CardModel source)
    {
        var state = await GetOrCreate(context, player.Creature, source);
        int stashed = 0;
        while (stashed < amount && state.Count < state.Capacity)
        {
            var candidates = PileType.Discard.GetPile(player).Cards.Where(card =>!card.Keywords.Contains(CardKeyword.Ethereal)).ToList();
            if (candidates.Count == 0)
                break;
            var card = player.RunState.Rng.CombatTargets.NextItem(candidates)!;
            if (!await state.Stash(context, card, source))
                break;
            stashed++;
        }
        return stashed;
    }
    public static async Task<CardModel? > MoveToDiscard(PlayerChoiceContext context, Creature creature, CardModel source)
    {
        var state = await GetOrCreate(context, creature, source);
        if (state._slots.Count == 0)
            return null;
        var card =(await CardSelectCmd.FromSimpleGrid(context, state._slots, creature.Player!, new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER"), 1))).FirstOrDefault();
        if (card == null ||!state._slots.Remove(card))
            return null;
        await CardPileCmd.Add(card, PileType.Discard);
        return card;
    }
    public static async Task < bool> SwapWithTopDraw(PlayerChoiceContext context, Creature creature, CardModel source)
    {
        var state = await GetOrCreate(context, creature, source);
        var drawPile = PileType.Draw.GetPile(creature.Player!);
        var topCard = drawPile.Cards.FirstOrDefault();
        if (state._slots.Count == 0 || topCard == null)
            return false;
        var stashedCard =(await CardSelectCmd.FromSimpleGrid(context, state._slots, creature.Player!, new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER"), 1))).FirstOrDefault();
        if (stashedCard == null ||!state._slots.Remove(stashedCard))
            return false;
        _ = await state.Stash(context, topCard, source);
        await CardPileCmd.Add(stashedCard, PileType.Discard);
        return true;
    }
    public static bool Has(Creature creature, Func<CardModel, bool > predicate) => creature.GetPower<RaccoonStashStatePower>()?.Slots.Any(predicate) == true;
    private async Task < bool> Stash(PlayerChoiceContext context, CardModel card, CardModel source)
    {
        if (_slots.Count >= Capacity || card.Keywords.Contains(CardKeyword.Ethereal) || card.Pile == null)
            return false;
        card.RemoveFromCurrentPile(true);
        _slots.Add(card);
        _stashedTurn = Owner.Player?.PlayerCombatState?.TurnNumber ?? - 1;
        MilestoneTracker.RecordRaccoonStashed(Owner, this);
        foreach (var listener in Listeners().ToList()) await listener.OnStashed(context, this, card, source);
        return true;
    }
    private async Task < bool> Spend(PlayerChoiceContext context, CardModel card, CardModel source)
    {
        if (!_slots.Remove(card))
            return false;
        SpentThisTurn = true;
        MilestoneTracker.RecordRaccoonSpent(Owner, card);
        // CardPileCmd.Add routes a full hand to discard.  Exhaust directly from the
        // detached Stash card so neither hand-add hooks nor a full-hand fallback can
        // create value or leave the card outside the Exhaust pile.
        await CardCmd.Exhaust(context, card);
        foreach (var listener in Listeners().ToList()) await listener.OnSpent(context, this, card, source);
        return true;
    }
    public override Task AfterPlayerTurnStart(PlayerChoiceContext context, Player player)
    {
        if (player == Owner.Player)
            SpentThisTurn = false;
        return Task.CompletedTask;
    }
    public override async Task AfterCombatEnd(CombatRoom room)
    {
        if (Owner.IsDead)
            return;
        foreach (var card in _slots.ToList()) await CardPileCmd.Add(card, PileType.Discard, skipVisuals : true);
        _slots.Clear();
    }
    private IEnumerable<IStashListener> Listeners() => Owner.Powers.OfType<IStashListener>().Concat(Owner.Player?.Relics.OfType<IStashListener>() ?? []);
}
public interface IStashListener
{
    Task OnStashed(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source) => Task.CompletedTask;
    Task OnSpent(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source) => Task.CompletedTask;
    Task OnUnstashed(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source) => Task.CompletedTask;
}
