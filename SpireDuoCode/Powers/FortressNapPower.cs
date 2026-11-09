using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class FortressNapPower : SpireDuoPower
{
    private int _lastTriggerTurn = -1;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.FromPower<PlatingPower>()];

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power is not DrowsyPower || power.Owner != Owner || amount <= 0) return;
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1;
        if (_lastTriggerTurn == turn) return;
        _lastTriggerTurn = turn;
        Flash();
        await PowerCmd.Apply<PlatingPower>(choiceContext, Owner, Amount, Owner, null);
    }
}
