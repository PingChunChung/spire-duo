using MegaCrit.Sts2.Core.Entities.Players;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Powers;

public enum Footprint
{
    Empty,
    Attack,
    Skill,
    Power
}

/// <summary>Combat-local Black Cat state. It is advanced only by replicated card-play hooks.</summary>
public sealed class FootprintStatePower : SpireDuoPower
{
    private readonly Footprint[] _slots = [Footprint.Empty, Footprint.Empty, Footprint.Empty];
    private readonly bool[] _locked = [false, false, false];
    private int _omenTurn = -1;

    public IReadOnlyList<Footprint> Slots => _slots;
    public IReadOnlyList<bool> Locked => _locked;
    public bool CompanionArmed { get; private set; }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

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
        state.Set(index, footprint);
    }

    public static async Task Lock(PlayerChoiceContext context, Creature creature, int index, CardModel? source = null)
    {
        var state = await GetOrCreate(context, creature, source);
        state._locked[index] = true;
    }

    public static async Task ArmCompanion(PlayerChoiceContext context, Creature creature, CardModel? source = null)
    {
        var state = await GetOrCreate(context, creature, source);
        state.CompanionArmed = true;
    }

    public void Set(int index, Footprint footprint)
    {
        if (!_locked[index])
            _slots[index] = footprint;
    }

    public void RotateRight()
    {
        var slots = _slots.ToArray();
        var locks = _locked.ToArray();
        for (int i = 0; i < 3; i++)
        {
            if (_locked[i])
                continue;

            int source = (i + 2) % 3;
            _slots[i] = slots[source];
            _locked[i] = locks[source] && !locks[i];
        }
    }

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

        if (CompanionArmed && footprint == Footprint.Attack)
            CompanionArmed = false;

        if (!_locked[0])
        {
            _slots[0] = _slots[1];
            _slots[1] = _slots[2];
            _slots[2] = footprint;
        }
        else if (!_locked[1])
        {
            _slots[1] = _slots[2];
            _slots[2] = footprint;
        }
        else if (!_locked[2])
        {
            _slots[2] = footprint;
        }
        else
        {
            return;
        }

        if (_slots.SequenceEqual([Footprint.Attack, Footprint.Skill, Footprint.Attack]))
        {
            await TriggerPounce(choiceContext, cardPlay);
        }
        else if (_slots.SequenceEqual([Footprint.Skill, Footprint.Skill, Footprint.Attack]))
        {
            await TriggerGroom(choiceContext, cardPlay);
        }
        else if (_slots.SequenceEqual([Footprint.Power, Footprint.Skill, Footprint.Power]))
        {
            await TriggerOmen(choiceContext, cardPlay);
        }
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
            Array.Fill(_locked, false);
        return Task.CompletedTask;
    }

    private async Task TriggerPounce(PlayerChoiceContext context, CardPlay cardPlay)
    {
        var enemies = Owner.CombatState!.HittableEnemies;
        if (enemies.Count > 0)
        {
            var enemy = Owner.Player!.RunState.Rng.CombatTargets.NextItem(enemies)!;
            await DamageCmd.Attack(8).FromCard(cardPlay.Card).Targeting(enemy).WithHitFx("vfx/vfx_attack_slash").Execute(context);
            await PowerCmd.Apply<VulnerablePower>(context, enemy, 1, Owner, cardPlay.Card);
        }
        Clear();
    }

    private async Task TriggerGroom(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner, 8, ValueProp.Unpowered, cardPlay);
        await CardPileCmd.Draw(context, 1, Owner.Player!);
        Clear();
    }

    private async Task TriggerOmen(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(1, Owner.Player!);
        await PowerCmd.Apply<RetainHandPower>(context, Owner, 1, Owner, cardPlay.Card);
        var velvet = Owner.GetPower<VelvetEclipsePower>();
        int turn = Owner.Player!.PlayerCombatState?.TurnNumber ?? -1;
        if (velvet != null && _omenTurn != turn)
        {
            _omenTurn = turn;
            await CardPileCmd.Draw(context, velvet.Amount, Owner.Player!);
        }
        Clear();
    }

    private void Clear()
    {
        Array.Fill(_slots, Footprint.Empty);
        Array.Fill(_locked, false);
        Flash();
    }
}
