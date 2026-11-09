namespace SpireDuo.SpireDuoCode.Powers;

public sealed class CandleAtNoonPower : SpireDuoPower
{
    private int _turn = -1;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1;
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card.Type != CardType.Power || turn == _turn)
            return;
        _turn = turn;
        Flash();
        await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, cardPlay);
    }
}
