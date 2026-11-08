namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever the owner wakes up, gain Strength. Triggered from <see cref="DrowsyPower.WakeUp"/>.</summary>
public sealed class GrumpyMorningPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.FromPower<StrengthPower>()];

    internal async Task OnWake(PlayerChoiceContext choiceContext)
    {
        Flash();
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, Amount, Owner, null);
    }
}
