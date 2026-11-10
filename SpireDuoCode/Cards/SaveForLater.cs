using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Progress;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SaveForLater() : KoalaCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6m, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        var selection = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1), context: choiceContext, player: Owner,
            filter: c => c != this && !c.Keywords.Contains(CardKeyword.Retain), source: this)).FirstOrDefault();
        if (selection != null)
        {
            selection.AddKeyword(CardKeyword.Retain);
            MilestoneTracker.RecordRetainGranted(Owner.Creature, selection);
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}
