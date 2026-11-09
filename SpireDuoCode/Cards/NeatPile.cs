namespace SpireDuo.SpireDuoCode.Cards;
public sealed class NeatPile() : BlackCatCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await CardPileCmd.Draw(c, DynamicVars.Cards.BaseValue, Owner);
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1m);
}
