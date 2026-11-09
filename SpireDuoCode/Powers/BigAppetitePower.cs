namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever the owner munches Bamboo, gain Strength. Triggered from <see cref="BambooPower.Munch"/>.</summary>
public sealed class BigAppetitePower : SpireDuoPower
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

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
}
