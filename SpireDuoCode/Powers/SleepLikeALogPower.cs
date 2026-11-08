namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>At the end of the owner's turn, gain extra Block equal to their Drowsy.</summary>
public sealed class SleepLikeALogPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner)) return;
        int drowsy = Owner.GetPowerAmount<DrowsyPower>();
        if (drowsy <= 0) return;
        Flash();
        await CreatureCmd.GainBlock(Owner, drowsy * Amount, ValueProp.Unpowered, null);
    }
}
