namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever the owner's Block stops an enemy attack, the attacker takes damage.</summary>
public sealed class BellyBouncePower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || result.BlockedDamage <= 0 || dealer == null || dealer.Side == Owner.Side || !props.IsPoweredAttack()) return;
        Flash();
        await CreatureCmd.Damage(choiceContext, dealer, Amount, ValueProp.Unpowered, Owner, null);
    }
}
