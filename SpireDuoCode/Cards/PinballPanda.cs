namespace SpireDuo.SpireDuoCode.Cards;
public sealed class PinballPanda() : PandaCard(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<RollPower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { int roll = await RollPower.SpendAll(Owner.Creature); await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(1 + roll / 2).FromCard(this).TargetingAllOpponents(CombatState!).Execute(c); }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2m);
}
