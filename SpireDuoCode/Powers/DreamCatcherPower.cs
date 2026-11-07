namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Every time the owner gains Drowsy, they gain this much more.</summary>
public sealed class DreamCatcherPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? applier, out decimal modifiedAmount)
    {
        if (target == Owner && canonicalPower is DrowsyPower && amount > 0)
        {
            modifiedAmount = amount + Amount;
            return true;
        }
        modifiedAmount = amount;
        return false;
    }
}
