using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Lockpick() : RaccoonCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        if (await RaccoonStashStatePower.Spend(c, Owner.Creature, this) != null)
            await CardPileCmd.Draw(c, IsUpgraded? 3 : 2, Owner);
    }
    protected override void OnUpgrade()
    {
    }
}
