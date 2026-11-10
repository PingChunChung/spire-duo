using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Rummage() : RaccoonCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await RaccoonStashStatePower.StashFromTopDraw(c, Owner, IsUpgraded? 5 : 3, this);
    }
    protected override void OnUpgrade()
    {
    }
}
