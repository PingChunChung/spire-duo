using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class DeepPocketsPower : SpireDuoPower, IStashListener
{
    private int _turn = - 1;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public Task OnStashed(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? - 1;
        if (_turn == turn)
            return Task.CompletedTask;
        _turn = turn;
        card.EnergyCost.SetThisTurn(Math.Min(3, RaccoonStashStatePower.CostValue(card) + 1));
        Flash();
        return Task.CompletedTask;
    }
}
