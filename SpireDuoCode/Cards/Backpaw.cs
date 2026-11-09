namespace SpireDuo.SpireDuoCode.Cards;
public sealed class Backpaw() : BlackCatCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move), new BlockVar(4m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { ArgumentNullException.ThrowIfNull(p.Target); await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(p.Target).WithHitFx("vfx/vfx_attack_slash").Execute(c); await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, p); }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2m);
}
