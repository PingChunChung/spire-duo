namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>At the start of the owner's turn, gain 1 Strength for every <see cref="Divisor"/> Drowsy.</summary>
public sealed class DreamPowerPower : SpireDuoPower
{
    public const int Divisor = 4;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.FromPower<StrengthPower>()];

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return;
        int strength = Owner.GetPowerAmount<DrowsyPower>() / Divisor * Amount;
        if (strength <= 0) return;
        Flash();
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner, strength, Owner, null);
    }
}
