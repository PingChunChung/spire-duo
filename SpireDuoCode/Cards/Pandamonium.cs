namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Pandamonium() : PandaCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.AutoPlayFromDrawPile(choiceContext, Owner, DynamicVars.Cards.IntValue, CardPilePosition.Top, forceExhaust: false);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}
