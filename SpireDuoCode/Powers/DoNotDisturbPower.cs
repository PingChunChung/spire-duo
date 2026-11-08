namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever an enemy attacks the owner, it takes damage equal to the owner's Drowsy.</summary>
public sealed class DoNotDisturbPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || dealer == null || dealer.Side == Owner.Side || !props.IsPoweredAttack()) return;
        int drowsy = Owner.GetPowerAmount<DrowsyPower>();
        if (drowsy <= 0) return;
        Flash();
        await CreatureCmd.Damage(choiceContext, dealer, drowsy * Amount, ValueProp.Unpowered, Owner, null);
    }
}
