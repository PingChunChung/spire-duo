namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>At the end of the owner's turn, gain Block for each Bamboo they have.</summary>
public sealed class HoarderPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>(), HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner)) return;
        int bamboo = Owner.GetPowerAmount<BambooPower>();
        if (bamboo <= 0) return;
        Flash();
        await CreatureCmd.GainBlock(Owner, bamboo * Amount, ValueProp.Unpowered, null);
    }
}
