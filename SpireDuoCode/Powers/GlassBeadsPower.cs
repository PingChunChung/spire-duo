using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class GlassBeadsPower : SpireDuoPower, IStashListener
{
    private int _turn = - 1;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public async Task OnSpent(PlayerChoiceContext c, RaccoonStashStatePower s, CardModel card, CardModel source)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? - 1;
        if (_turn == turn || RaccoonStashStatePower.CostValue(card) != 0)
            return;
        _turn = turn;
        await PowerCmd.Apply<DexterityPower>(c, Owner, Amount, Owner, source);
    }
}
