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

    /// <summary>Removes all Drowsiness from the creature and returns how much there was.</summary>
    public static async Task<int> WakeUp(Creature creature)
    {
        int amount = creature.GetPowerAmount<DrowsyPower>();
        if (amount > 0) await PowerCmd.Remove<DrowsyPower>(creature);
        return amount;
    }
}
