namespace SpireDuo.SpireDuoCode.Cards;

public sealed class VelvetAmbush() : BlackCatCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12m, ValueProp.Move), new DynamicVar("FullFootprintsDamage", 20m)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) { ArgumentNullException.ThrowIfNull(play.Target); var state = await FootprintStatePower.GetOrCreate(context, Owner.Creature, this); decimal damage = state.Slots.All(slot => slot != Footprint.Empty) ? DynamicVars["FullFootprintsDamage"].BaseValue : DynamicVars.Damage.BaseValue; await DamageCmd.Attack(damage).FromCard(this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(context); }
    protected override void OnUpgrade() { DynamicVars.Damage.UpgradeValueBy(3m); DynamicVars["FullFootprintsDamage"].UpgradeValueBy(4m); }
}
