namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Munch costs are reduced, to a minimum of 1. Read by <see cref="BambooPower.Munch"/>.</summary>
public sealed class BottomlessStomachPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];
}
