namespace SpireDuo.SpireDuoCode.Cards;
public sealed class SharedPerch() : BlackCatCard(1, CardType.Skill, CardRarity.Common, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new DynamicVar("CompanionBlock", 5m)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { if (p.Target != null && !p.Target.IsDead) { await CardPileCmd.Draw(c, DynamicVars.Cards.BaseValue, p.Target.Player!); await FootprintStatePower.ArmCompanion(c, Owner.Creature, p.Target, CardType.Power, DynamicVars["CompanionBlock"].BaseValue, this); } await FootprintStatePower.Lock(c, Owner.Creature, 0, this); }
    protected override void OnUpgrade() { DynamicVars.Cards.UpgradeValueBy(1m); DynamicVars["CompanionBlock"].UpgradeValueBy(3m); }
}
