namespace SpireDuo.SpireDuoCode.Cards;

public sealed class TwinMoon() : BlackCatCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Owner);
        if (play.Target?.Player is { } ally && !play.Target.IsDead)
            await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, ally);
        await FootprintStatePower.Set(context, Owner.Creature, 1, Footprint.Skill, this);
    }
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1m);
}
