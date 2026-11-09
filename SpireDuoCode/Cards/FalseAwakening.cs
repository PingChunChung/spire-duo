namespace SpireDuo.SpireDuoCode.Cards;

public sealed class FalseAwakening() : KoalaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (var dream in PileType.Hand.GetPile(Owner).Cards.OfType<Dream>().ToList())
        {
            await dream.Resolve(choiceContext, cardPlay);
            await CardCmd.Exhaust(choiceContext, dream);
        }
        for (int i = 0; i < DynamicVars.Cards.IntValue; i++) await Dream.AddToHand(choiceContext, Owner, IsUpgraded);
    }
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1m);
}
