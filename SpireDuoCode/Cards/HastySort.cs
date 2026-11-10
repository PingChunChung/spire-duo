using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class HastySort() : RaccoonCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await RaccoonCardEffects.Stash(c, this);
        if (IsUpgraded)
            await CardPileCmd.Draw(c, 1, Owner);
        var discarded =(await CardSelectCmd.FromHandForDiscard(c, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1), null, this)).FirstOrDefault();
        if (discarded != null)
            await CardPileCmd.Add(discarded, PileType.Discard);
    }
    protected override void OnUpgrade()
    {
    }
}
