using MegaCrit.Sts2.Core.CardSelection;

namespace SpireDuo.SpireDuoCode.Cards;
public sealed class Slink() : BlackCatCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await CardPileCmd.Draw(c, DynamicVars.Cards.BaseValue, Owner);
        var discarded = (await CardSelectCmd.FromHandForDiscard(c, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1), null, this)).FirstOrDefault();
        if (discarded != null)
            await CardPileCmd.Add(discarded, PileType.Discard);
    }
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1m);
}
