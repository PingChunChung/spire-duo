namespace SpireDuo.SpireDuoCode.Cards;

public sealed class PartnerInCrime() : PandaCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PartnerInCrimePower>(3m)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<PartnerInCrimePower>(choiceContext, Owner.Creature, DynamicVars["PartnerInCrimePower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["PartnerInCrimePower"].UpgradeValueBy(2m);
    }
}
