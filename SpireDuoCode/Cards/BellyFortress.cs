namespace SpireDuo.SpireDuoCode.Cards;
public sealed class BellyFortress() : PandaCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BellyFortressPower>(3m)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) => await PowerCmd.Apply<BellyFortressPower>(choiceContext, Owner.Creature, DynamicVars["BellyFortressPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["BellyFortressPower"].UpgradeValueBy(2m);
}
