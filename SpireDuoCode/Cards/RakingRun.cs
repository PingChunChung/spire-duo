namespace SpireDuo.SpireDuoCode.Cards;
public sealed class RakingRun() : BlackCatCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5m, ValueProp.Move), new RepeatVar(2)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { ArgumentNullException.ThrowIfNull(p.Target); await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(DynamicVars.Repeat.IntValue).FromCard(this).Targeting(p.Target).WithHitFx("vfx/vfx_attack_slash").Execute(c); }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(1m);
}
