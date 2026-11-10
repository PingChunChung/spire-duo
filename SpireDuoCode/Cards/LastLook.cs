using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class LastLook() : RaccoonCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await RaccoonStashStatePower.StashFromDiscard(c, Owner, this);
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(- 1);
}
