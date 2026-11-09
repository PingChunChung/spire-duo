namespace SpireDuo.SpireDuoCode.Powers;

public sealed class LanternShadowPower : FootprintListenerPower
{
    private int _turn = -1;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override async Task OnFootprintLine(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1;
        if (line != FootprintLine.Omen || turn == _turn)
            return;
        _turn = turn;
        Flash();
        await PowerCmd.Apply<ArtifactPower>(context, Owner, 1, Owner, cardPlay.Card);
        if (Amount > 1)
            await CardPileCmd.Draw(context, 1, Owner.Player!);
    }
}
