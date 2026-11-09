namespace SpireDuo.SpireDuoCode.Cards;

public sealed class PolishedClaws() : BlackCatCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move), new DynamicVar("GroomDamage", 16m)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) { ArgumentNullException.ThrowIfNull(play.Target); var state = await FootprintStatePower.GetOrCreate(context, Owner.Creature, this); decimal damage = state.Slots[0] == Footprint.Skill && state.Slots[1] == Footprint.Skill ? DynamicVars["GroomDamage"].BaseValue : DynamicVars.Damage.BaseValue; await DamageCmd.Attack(damage).FromCard(this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(context); }
    protected override void OnUpgrade() { DynamicVars.Damage.UpgradeValueBy(2m); DynamicVars["GroomDamage"].UpgradeValueBy(4m); }
}
