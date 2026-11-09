namespace SpireDuo.SpireDuoCode.Cards;
public sealed class TwoBeatSignal() : BlackCatCard(1, CardType.Skill, CardRarity.Common, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("AllyBlock", 6m), new DynamicVar("CompanionBlock", 4m)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { if (p.Target != null && !p.Target.IsDead) { await CreatureCmd.GainBlock(p.Target, DynamicVars["AllyBlock"].BaseValue, ValueProp.Move, p); await FootprintStatePower.ArmCompanion(c, Owner.Creature, p.Target, CardType.Attack, DynamicVars["CompanionBlock"].BaseValue, this); } await FootprintStatePower.Set(c, Owner.Creature, 1, Footprint.Skill, this); }
    protected override void OnUpgrade() { DynamicVars["AllyBlock"].UpgradeValueBy(3m); DynamicVars["CompanionBlock"].UpgradeValueBy(2m); }
}
