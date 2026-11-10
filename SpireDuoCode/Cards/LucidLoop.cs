using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Progress;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class LucidLoop() : KoalaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain), EnergyHoverTip];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var selected = (await CardSelectCmd.FromHand(
            context: choiceContext,
            player: Owner,
            prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1),
            filter: c => c != this,
            source: this)).FirstOrDefault();
        if (selected == null) return;
        bool retained = selected.Keywords.Contains(CardKeyword.Retain);
        selected.AddKeyword(CardKeyword.Retain);
        if (!retained)
            MilestoneTracker.RecordRetainGranted(Owner.Creature, selected);
        if (retained) selected.EnergyCost.SetUntilPlayed(Math.Max(0, selected.EnergyCost.GetAmountToSpend() - DynamicVars.Energy.IntValue), reduceOnly: true);
    }
    protected override void OnUpgrade() => DynamicVars.Energy.UpgradeValueBy(1m);
}
