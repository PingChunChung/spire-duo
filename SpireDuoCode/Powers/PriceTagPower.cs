using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class PriceTagPower : SpireDuoPower, IStashListener
{
    private int _turn = - 1;
    private bool _upgraded;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        _upgraded = cardSource?.IsUpgraded == true;
        return Task.CompletedTask;
    }
    public async Task OnSpent(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? - 1;
        if (_turn == turn || RaccoonStashStatePower.CostValue(card) < 2)
            return;
        _turn = turn;
        Flash();
        await PlayerCmd.GainEnergy((int) Amount, Owner.Player!);
        if (_upgraded)
            await CardPileCmd.Draw(context, 1, Owner.Player!);
    }
}
