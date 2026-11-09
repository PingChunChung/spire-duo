namespace SpireDuo.SpireDuoCode.Cards;

public sealed class FleaComb() : BlackCatCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move), new PowerVar<StrengthPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) { ArgumentNullException.ThrowIfNull(play.Target); await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(context); await PowerCmd.Apply<StrengthPower>(context, play.Target, -DynamicVars["StrengthPower"].BaseValue, Owner.Creature, this); }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m);
}
