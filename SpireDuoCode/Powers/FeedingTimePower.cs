namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>At the start of the owner's turn, Munch 2: every other player on their side gains Energy.
/// Runs after every player's energy reset, so the gift is not wiped.</summary>
public sealed class FeedingTimePower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>(), HoverTipFactory.ForEnergy(this)];

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return;
        var teammates = CombatState.GetTeammatesOf(Owner).Where(c => c != Owner && c.IsAlive && c.Player != null).ToList();
        if (teammates.Count == 0) return;
        if (!await BambooPower.Munch(new ThrowingPlayerChoiceContext(), Owner, 2, null)) return;
        Flash();
        foreach (var teammate in teammates)
            await PlayerCmd.GainEnergy(Amount, teammate.Player!);
    }
}
