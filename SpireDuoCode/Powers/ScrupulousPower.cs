namespace SpireDuo.SpireDuoCode.Powers;

public sealed class ScrupulousPower : FootprintListenerPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task OnFootprintLine(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line)
    {
        if (line != FootprintLine.Groom)
            return;

        Flash();
        await PowerCmd.Apply<DexterityPower>(context, Owner, Amount, Owner, cardPlay.Card);
        await PowerCmd.Apply<ScrupulousDexterityPower>(context, Owner, Amount, Owner, cardPlay.Card);
    }
}
