namespace SpireDuo.SpireDuoCode.Powers;

public sealed class OmenReaderPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card.Type != CardType.Power)
            return;
        Flash();
        await CardPileCmd.Draw(context, Amount, Owner.Player!);
    }
}
