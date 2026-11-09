namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DreamGate() : KoalaCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DreamGatePower>(1m)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) =>
        await PowerCmd.Apply<DreamGatePower>(choiceContext, Owner.Creature, DynamicVars["DreamGatePower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["DreamGatePower"].UpgradeValueBy(1m);
}
