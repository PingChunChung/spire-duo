namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever the owner wakes up, gain Energy. Triggered from <see cref="DrowsyPower.WakeUp"/>.</summary>
public sealed class MorningPersonPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.ForEnergy(this)];

    internal async Task OnWake()
    {
        if (Owner.Player == null) return;
        Flash();
        await PlayerCmd.GainEnergy(Amount, Owner.Player);
    }
}
