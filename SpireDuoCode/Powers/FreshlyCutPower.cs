namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>One-turn damage bonus granted by the first successful Freshly-Cut Munch.</summary>
public sealed class FreshlyCutPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource) =>
        dealer == Owner && props.IsPoweredAttack() ? Amount : 0m;

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner)) await PowerCmd.Remove(this);
    }
}
