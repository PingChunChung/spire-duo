using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class MasterThiefPower : SpireDuoPower, IStashListener
{
    private int _turn = - 1;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public async Task OnStashed(PlayerChoiceContext c, RaccoonStashStatePower s, CardModel card, CardModel source)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? - 1;
        if (_turn == turn || s.Count != s.Capacity)
            return;
        _turn = turn;
        await PlayerCmd.GainEnergy(1, Owner.Player!);
        await CardPileCmd.Draw(c, 1, Owner.Player!);
    }
}
