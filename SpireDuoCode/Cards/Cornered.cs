namespace SpireDuo.SpireDuoCode.Cards;
public sealed class Cornered() : BlackCatCard(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10m, ValueProp.Move), new PowerVar<VulnerablePower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { ArgumentNullException.ThrowIfNull(p.Target); await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(p.Target).WithHitFx("vfx/vfx_attack_blunt").Execute(c); await PowerCmd.Apply<VulnerablePower>(c, p.Target, DynamicVars["VulnerablePower"].BaseValue, Owner.Creature, this); }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m);
}
