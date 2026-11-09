namespace SpireDuo.SpireDuoCode.Cards;
public sealed class SecondCourse() : PandaCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10m, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FullPower>(), HoverTipFactory.FromPower<BambooPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) { ArgumentNullException.ThrowIfNull(cardPlay.Target); await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).Execute(choiceContext); if (await FullPower.Consume(choiceContext, Owner.Creature, this) && await BambooPower.Munch(choiceContext, Owner.Creature, 2, this)) await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).Execute(choiceContext); }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m);
}
