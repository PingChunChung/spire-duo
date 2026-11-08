using SpireDuo.SpireDuoCode.Relics;

namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Koala's resource. Gives Block at the end of every turn; "wake" cards spend all of it at once.</summary>
public sealed class DrowsyPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task BeforeSideTurnEndEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner)) return;
        Flash();
        await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
    }

    /// <summary>Wake: removes all Drowsiness from the creature, triggers wake effects and returns how much there was.</summary>
    public static async Task<int> WakeUp(PlayerChoiceContext choiceContext, Creature creature)
    {
        int amount = creature.GetPowerAmount<DrowsyPower>();
        if (amount <= 0) return 0;
        await PowerCmd.Remove<DrowsyPower>(creature);
        var temper = creature.GetPower<ShortTemperPower>();
        if (temper != null) await temper.OnWake(choiceContext, amount);
        var clock = creature.Player?.GetRelic<AlarmClock>();
        if (clock != null) await clock.OnWake(choiceContext);
        var snooze = creature.Player?.GetRelic<FiveMoreMinutes>();
        if (snooze != null) await snooze.OnWake(choiceContext, amount);
        return amount;
    }
}
