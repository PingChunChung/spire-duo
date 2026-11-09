namespace SpireDuo.SpireDuoCode.Cards;

public sealed class MoonlitLunge() : BlackCatCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(15m, ValueProp.Move), new DynamicVar("PounceDamage", 24m)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) { ArgumentNullException.ThrowIfNull(play.Target); decimal damage = Owner.Creature.GetPower<FootprintStatePower>()?.HasTriggered(FootprintLine.Pounce) == true ? DynamicVars["PounceDamage"].BaseValue : DynamicVars.Damage.BaseValue; await DamageCmd.Attack(damage).FromCard(this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(context); }
    protected override void OnUpgrade() { DynamicVars.Damage.UpgradeValueBy(3m); DynamicVars["PounceDamage"].UpgradeValueBy(4m); }
}
