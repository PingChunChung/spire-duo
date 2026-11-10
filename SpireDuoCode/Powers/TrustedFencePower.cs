using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class TrustedFencePower : SpireDuoPower, IStashListener
{
    private int _turn = - 1;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public async Task OnSpent(PlayerChoiceContext c, RaccoonStashStatePower s, CardModel card, CardModel source)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? - 1;
        if (_turn == turn)
            return;
        var ally = RaccoonStashStatePower.DeterministicAlly(Owner);
        if (ally == null)
            return;
        _turn = turn;
        await CardPileCmd.Draw(c,(int) Amount, ally);
    }
}
