namespace SpireDuo.SpireDuoCode.Powers;

public sealed class CatnapLedgerPower : SpireDuoPower
{
    private int _turn = -1;
    private int _skillsPlayed;
    private bool _triggered;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
            return;

        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1;
        if (turn != _turn)
        {
            _turn = turn;
            _skillsPlayed = 0;
            _triggered = false;
        }
        if (cardPlay.Card.Type != CardType.Skill || _triggered)
            return;

        _skillsPlayed++;
        if (_skillsPlayed != 2)
            return;

        _triggered = true;
        Flash();
        await CardPileCmd.Draw(context, 1, Owner.Player!);
        if (Amount > 1)
            await CreatureCmd.GainBlock(Owner, 2, ValueProp.Unpowered, cardPlay);
    }
}
