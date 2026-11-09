namespace SpireDuo.SpireDuoCode.Cards;
public sealed class CleanBreak() : BlackCatCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move), new CardsVar(1)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { ArgumentNullException.ThrowIfNull(p.Target); await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(p.Target).WithHitFx("vfx/vfx_attack_slash").Execute(c); if (Owner.Creature.GetPower<FootprintStatePower>()?.Slots[0] == Footprint.Skill) await CardPileCmd.Draw(c, 1, Owner); }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2m);
}
