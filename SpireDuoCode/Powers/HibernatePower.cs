namespace SpireDuo.SpireDuoCode.Powers;

public sealed class HibernatePower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return;
        Flash();
        await PowerCmd.Apply<DrowsyPower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
    }
}
