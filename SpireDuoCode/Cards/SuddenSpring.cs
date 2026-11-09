namespace SpireDuo.SpireDuoCode.Cards;
public sealed class SuddenSpring() : BlackCatCard(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { ArgumentNullException.ThrowIfNull(p.Target); await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(p.Target).WithHitFx("vfx/vfx_attack_slash").Execute(c); }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2m);
}
