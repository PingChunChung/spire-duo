using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class QuietTradePower : SpireDuoPower, IStashListener
{
    private int _turn = - 1;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public async Task OnSpent(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? - 1;
        if (_turn == turn)
            return;
        var ally = RaccoonStashStatePower.DeterministicAlly(Owner);
        if (ally == null)
            return;
        _turn = turn;
        Flash();
        await CreatureCmd.GainBlock(ally.Creature, Amount, ValueProp.Unpowered, null);
    }
}
