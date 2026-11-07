namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>
/// Pincer's mark on an enemy: the next attack hit on it from an ally of the applier deals Amount extra damage.
/// Removed after that hit, or at the end of the players' turn.
/// </summary>
public sealed class PincerPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource) =>
        IsAllyHit(target, props, dealer) ? Amount : 0m;

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!IsAllyHit(target, props, dealer)) return;
        Flash();
        await PowerCmd.Remove(this);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player) await PowerCmd.Remove(this);
    }

    private bool IsAllyHit(Creature? target, ValueProp props, Creature? dealer) =>
        target == Owner && props.IsPoweredAttack() && dealer != null && Applier != null && dealer.Side == Applier.Side
        && dealer != Applier && dealer.PetOwner?.Creature != Applier;
}
