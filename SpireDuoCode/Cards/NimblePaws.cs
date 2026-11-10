using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class NimblePaws() : RaccoonCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        var card = await RaccoonStashStatePower.Unstash(c, Owner.Creature, this);
        card?.EnergyCost.SetThisTurn(Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.Local) - 1));
        if (IsUpgraded)
            await CardPileCmd.Draw(c, 1, Owner);
    }
    protected override void OnUpgrade()
    {
    }
}
