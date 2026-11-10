using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class MoonBazaar() : RaccoonCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
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
