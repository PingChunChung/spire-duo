using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class CounterfeitMap() : RaccoonCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        var card = await RaccoonStashStatePower.Unstash(c, Owner.Creature, this);
        if (card != null)
        {
            card.EnergyCost.SetThisTurn(0);
            await CardPileCmd.Draw(c, IsUpgraded? 2 : 1, Owner);
        }
    }
    protected override void OnUpgrade()
    {
    }
}
