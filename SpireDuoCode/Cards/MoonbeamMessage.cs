namespace SpireDuo.SpireDuoCode.Cards;

public sealed class MoonbeamMessage() : BlackCatCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new CardsVar(1)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        if (play.Target?.Player is { } ally && !play.Target.IsDead)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, ally);
            if (IsUpgraded)
                await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, ally);
        }
        await FootprintStatePower.Set(context, Owner.Creature, 0, Footprint.Power, this);
    }
    protected override void OnUpgrade() { }
}
