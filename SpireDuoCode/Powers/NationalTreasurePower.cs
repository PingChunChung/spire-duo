namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>At the start of the owner's turn, gain Strength and Dexterity.</summary>
public sealed class NationalTreasurePower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<DexterityPower>()];

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return;
        Flash();
        var choiceContext = new ThrowingPlayerChoiceContext();
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, Amount, Owner, null);
        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner, Amount, Owner, null);
    }
}
