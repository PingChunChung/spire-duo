using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SecondPocket() : RaccoonCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await RaccoonStashStatePower.SwapWithTopDraw(c, Owner.Creature, this);
        if (IsUpgraded)
            await CardPileCmd.Draw(c, 1, Owner);
    }
    protected override void OnUpgrade()
    {
    }
}
