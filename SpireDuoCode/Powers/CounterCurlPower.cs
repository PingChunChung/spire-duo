namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>One-turn counter: the next enemy attack that removes Block damages all enemies.</summary>
public sealed class CounterCurlPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || result.BlockedDamage <= 0 || dealer == null || dealer.Side == Owner.Side || !props.IsPoweredAttack()) return;
        Flash();
        await CreatureCmd.Damage(choiceContext, Owner.CombatState!.HittableEnemies, Amount, ValueProp.Unpowered, Owner, null);
        await PowerCmd.Remove(this);
    }
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner)) await PowerCmd.Remove(this);
    }
}
