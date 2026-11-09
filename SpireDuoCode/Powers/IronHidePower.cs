namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever the owner munches Bamboo, gain Block. Triggered from <see cref="BambooPower.Munch"/>.</summary>
public sealed class IronHidePower : SpireDuoPower
{
    private class Data { public int turn; }
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    internal void FlashIcon() => Flash();

    protected override object InitInternalData() => new Data();

    internal bool TryTriggerThisTurn()
    {
        var data = GetInternalData<Data>();
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? 0;
        if (data.turn == turn) return false;
        data.turn = turn;
        return true;
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];
}
