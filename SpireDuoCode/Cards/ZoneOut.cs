using MegaCrit.Sts2.Core.CardSelection;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class ZoneOut() : KoalaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrowsyPower>(3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = (await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 0, 999999999), null, this)).ToList();
        if (cards.Count == 0) return;
        await CardCmd.Discard(choiceContext, cards);
        await PowerCmd.Apply<DrowsyPower>(choiceContext, Owner.Creature, cards.Count * DynamicVars["DrowsyPower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["DrowsyPower"].UpgradeValueBy(1m);
    }
}
