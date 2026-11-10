using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class FalseIdentityPower : SpireDuoPower, IStashListener
{
    private int _turn = - 1;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public async Task OnUnstashed(PlayerChoiceContext c, RaccoonStashStatePower s, CardModel card, CardModel source)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? - 1;
        if (_turn == turn)
            return;
        _turn = turn;
        var copy = card.CreateClone();
        copy.EnergyCost.SetThisTurn(0);
        copy.ExhaustOnNextPlay = true;
        await CardPileCmd.AddGeneratedCardToCombat(copy, PileType.Hand, Owner.Player!);
    }
}
