namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever the owner munches Bamboo, gain Block. Triggered from <see cref="BambooPower.Munch"/>.</summary>
public sealed class IronHidePower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    internal void FlashIcon() => Flash();

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];
}
