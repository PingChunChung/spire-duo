namespace SpireDuo.SpireDuoCode.Cards;

public sealed class RooftopChase() : BlackCatCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy)
{
    protected override bool HasEnergyCostX => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) { var state = await FootprintStatePower.GetOrCreate(context, Owner.Creature, this); for (int i = 0; i < ResolveEnergyXValue(); i++) { await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingRandomOpponents(CombatState!).WithHitFx("vfx/vfx_attack_slash").Execute(context); await state.RotateRight(context, this); } }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(1m);
}
