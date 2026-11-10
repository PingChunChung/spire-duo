namespace SpireDuo.SpireDuoCode.Powers;

public sealed class BorrowedLanternPower : SpireDuoPower, IStashListener
{
    private int _turn = - 1;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public async Task OnStashed(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? - 1;
        if (_turn == turn)
            return;
        _turn = turn;
        Flash();
        await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
    }
}
