namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever the owner gains Drowsy, they gain that much Block.</summary>
public sealed class WarmAndFuzzyPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power is not DrowsyPower || power.Owner != Owner || amount <= 0) return;
        Flash();
        await CreatureCmd.GainBlock(Owner, amount * Amount, ValueProp.Unpowered, null);
    }
}
