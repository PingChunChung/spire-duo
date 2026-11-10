using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class CataloguePower : SpireDuoPower, IStashListener
{
    private int _turn = - 1;
    private bool _upgraded;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        _upgraded = cardSource?.IsUpgraded == true;
        return Task.CompletedTask;
    }
    public async Task OnUnstashed(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? - 1;
        if (_turn == turn)
            return;
        _turn = turn;
        Flash();
        await CardPileCmd.Draw(context,(int) Amount, Owner.Player!);
        if (_upgraded) card.EnergyCost.SetThisTurn(Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.Local)
            - 1));
    }
}
