using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class GreedyGlancePower : SpireDuoPower, IStashListener
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
    public async Task OnStashed(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? - 1;
        if (_turn == turn || RaccoonStashStatePower.CostValue(card) < 3)
            return;
        _turn = turn;
        Flash();
        await CardPileCmd.Draw(context,(int) Amount, Owner.Player!);
        if (_upgraded)
            await CreatureCmd.GainBlock(Owner, 2m, ValueProp.Unpowered, null);
    }
}
