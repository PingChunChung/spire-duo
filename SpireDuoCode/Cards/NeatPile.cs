using MegaCrit.Sts2.Core.CardSelection;

namespace SpireDuo.SpireDuoCode.Cards;
public sealed class NeatPile() : BlackCatCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        var selected = await CardSelectCmd.FromHand(c, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1, DynamicVars.Cards.IntValue), null, this);
        foreach (var card in selected)
            await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top);
    }
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1m);
}
