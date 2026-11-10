using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Progress;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DreamWithinDream() : KoalaCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var selection = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1), context: choiceContext, player: Owner,
            filter: c => c != this, source: this)).FirstOrDefault();
        if (selection == null) return;
        var copy = selection.CreateClone();
        copy.AddKeyword(CardKeyword.Retain);
        MilestoneTracker.RecordRetainGranted(Owner.Creature, copy);
        await CardPileCmd.AddGeneratedCardToCombat(copy, PileType.Hand, Owner);
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
