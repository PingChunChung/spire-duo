namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Strength counts one more time for the owner's attacks.</summary>
public sealed class HulkingPawsPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (dealer != Owner || !props.IsPoweredAttack()) return 0m;
        return Owner.GetPowerAmount<StrengthPower>() * Amount;
    }
}
