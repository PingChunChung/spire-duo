namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DaringDescent() : BlackCatCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move), new CardsVar(2)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) { ArgumentNullException.ThrowIfNull(play.Target); await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(context); if (play.Target.GetPower<VulnerablePower>() != null) await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Owner); }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2m);
}
