namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever an enemy attack makes the owner lose HP, gain Strength.</summary>
public sealed class FuryOfTheFluffyPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || result.UnblockedDamage <= 0 || dealer == null || dealer.Side == Owner.Side || !props.IsPoweredAttack()) return;
        Flash();
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, Amount, Owner, null);
    }
}
