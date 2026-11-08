namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever a card gives the owner Block, gain Bamboo.</summary>
public sealed class BambooShootsPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>(), HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        if (creature != Owner || amount <= 0 || cardSource == null) return;
        Flash();
        await PowerCmd.Apply<BambooPower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
    }
}
