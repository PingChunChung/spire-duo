using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class MidnightHaul() : RaccoonCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        int stashed = await RaccoonStashStatePower.StashFromHand(c, Owner, 2, this, minimum : 0);
        await CardPileCmd.Draw(c, stashed, Owner);
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(- 1);
}
