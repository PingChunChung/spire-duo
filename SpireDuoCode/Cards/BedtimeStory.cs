namespace SpireDuo.SpireDuoCode.Cards;

using SpireDuo.SpireDuoCode.Progress;

public sealed class BedtimeStory() : KoalaCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var drawn = await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        foreach (var card in drawn.ToList())
        {
            card.AddKeyword(CardKeyword.Retain);
            MilestoneTracker.RecordRetainGranted(Owner.Creature, card);
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}
