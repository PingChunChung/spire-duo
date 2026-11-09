namespace SpireDuo.SpireDuoCode.Powers;

public sealed class BorrowedRhythmPower : SpireDuoPower
{
    private bool _used;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
            return Task.CompletedTask;
        int cost = cardPlay.Card.EnergyCost.GetAmountToSpend();
        cardPlay.Card.EnergyCost.SetUntilPlayed(Math.Max(0, cost - Amount), reduceOnly: true);
        _used = true;
        Flash();
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (!_used || cardPlay.Card.Owner != Owner.Player)
            return;
        await PowerCmd.Remove(this);
    }
}
