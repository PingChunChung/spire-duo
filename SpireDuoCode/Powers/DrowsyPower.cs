using SpireDuo.SpireDuoCode.Relics;
using SpireDuo.SpireDuoCode.Progress;

namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Koala's resource. Gives Block at the end of every turn; "wake" cards spend all of it at once.</summary>
public sealed class DrowsyPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power != this || amount <= 0 || Owner.Player == null) return;
        MilestoneTracker.RecordDrowsy(Owner, Amount, amount);
        await PassingNote.OnResourceGained(choiceContext, Owner.Player);
    }

    public override async Task BeforeSideTurnEndEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner)) return;
        MilestoneTracker.RecordDrowsyTurnEnd(Owner, Amount);
        Flash();
        await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
    }

    /// <summary>Wake: removes all Drowsiness from the creature, triggers wake effects and returns how much there was.</summary>
    public static async Task<int> WakeUp(PlayerChoiceContext choiceContext, Creature creature)
    {
        int amount = creature.GetPowerAmount<DrowsyPower>();
        if (amount <= 0) return 0;
        MilestoneTracker.RecordWake(creature, amount);
        await PowerCmd.Remove<DrowsyPower>(creature);
        await PowerCmd.Apply<WakeMemoryPower>(choiceContext, creature, 1, creature, null);
        var temper = creature.GetPower<ShortTemperPower>();
        if (temper != null) await temper.OnWake(choiceContext, amount);
        var morning = creature.GetPower<MorningPersonPower>();
        if (morning != null) await morning.OnWake();
        var grumpy = creature.GetPower<GrumpyMorningPower>();
        if (grumpy != null) await grumpy.OnWake(choiceContext);
        var clock = creature.Player?.GetRelic<AlarmClock>();
        if (clock != null) await clock.OnWake(choiceContext);
        var snooze = creature.Player?.GetRelic<FiveMoreMinutes>();
        if (snooze != null) await snooze.OnWake(choiceContext, amount);
        var bell = creature.Player?.GetRelic<DreamCatcherBell>();
        if (bell != null) await bell.OnWake(choiceContext);
        return amount;
    }

    public static bool WokeThisTurn(Creature creature) => creature.GetPower<WakeMemoryPower>() != null;
}
