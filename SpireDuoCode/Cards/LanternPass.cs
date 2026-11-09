namespace SpireDuo.SpireDuoCode.Cards;

public sealed class LanternPass() : BlackCatCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new DynamicVar("CompanionBlock", 8m), new CardsVar(1)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        if (play.Target?.Player is { } ally && !play.Target.IsDead)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, ally);
            await FootprintStatePower.ArmCompanion(context, Owner.Creature, play.Target, CardType.Skill, DynamicVars["CompanionBlock"].BaseValue, this);
            if (IsUpgraded)
                await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, ally);
        }
    }
    protected override void OnUpgrade() { }
}
