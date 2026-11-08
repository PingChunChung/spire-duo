namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>The owner's first Munch(es) each turn cost no Bamboo. Checked from <see cref="BambooPower.Munch"/>.</summary>
public sealed class SecondStomachPower : SpireDuoPower
{
    private class Data
    {
        public int turn;
        public int used;
    }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];

    protected override object InitInternalData() => new Data();

    /// <summary>Uses up one free Munch for this turn if any are left.</summary>
    internal bool TryUseFreeMunch()
    {
        var data = GetInternalData<Data>();
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? 0;
        if (data.turn != turn)
        {
            data.turn = turn;
            data.used = 0;
        }
        if (data.used >= Amount) return false;
        data.used++;
        Flash();
        return true;
    }
}
