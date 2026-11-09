namespace SpireDuo.SpireDuoCode.Cards;
public sealed class TwoBeatSignal() : BlackCatCard(1, CardType.Skill, CardRarity.Common, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6m, ValueProp.Move), new DynamicVar("AllyBlock", 4m)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, p); if (p.Target != null && !p.Target.IsDead) await CreatureCmd.GainBlock(p.Target, DynamicVars["AllyBlock"].BaseValue, ValueProp.Move, p); await FootprintStatePower.Set(c, Owner.Creature, 2, Footprint.Skill, this); await FootprintStatePower.ArmCompanion(c, Owner.Creature, this); }
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3m);
}
