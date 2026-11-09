namespace SpireDuo.SpireDuoCode.Cards;
public sealed class SharedPerch() : BlackCatCard(1, CardType.Skill, CardRarity.Common, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new BlockVar(5m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { if (p.Target != null && !p.Target.IsDead) await CardPileCmd.Draw(c, DynamicVars.Cards.BaseValue, p.Target.Player!); await FootprintStatePower.ArmCompanion(c, Owner.Creature, this); if (Owner.Creature.GetPower<FootprintStatePower>()?.Slots[0] == Footprint.Power && p.Target != null && !p.Target.IsDead) await CreatureCmd.GainBlock(p.Target, DynamicVars.Block, p); }
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1m);
}
