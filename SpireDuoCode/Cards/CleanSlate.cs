using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class CleanSlate() : RaccoonCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        while (Owner.Creature.GetPower<RaccoonStashStatePower>()?.Count > 0)
        {
            var card = await RaccoonStashStatePower.Unstash(c, Owner.Creature, this);
            if (card == null)
                break;
            card.EnergyCost.SetThisTurn(Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.Local) - 1));
        }
        if (IsUpgraded)
            await CardPileCmd.Draw(c, 1, Owner);
    }
    protected override void OnUpgrade()
    {
    }
}
