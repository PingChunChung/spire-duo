using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SecondBell() : BlackCatCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await FootprintStatePower.UnlockAll(context, Owner.Creature, this);
        await FootprintStatePower.CopyNonEmptySlotsIntoAdjacentEmptySlots(context, Owner.Creature, this);
        if (IsUpgraded)
            await CardPileCmd.Draw(context, 2, Owner);
    }

    protected override void OnUpgrade()
    {
    }
}
