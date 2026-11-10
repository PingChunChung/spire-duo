using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SealedEnvelope() : RaccoonCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        int stashed = await RaccoonCardEffects.Stash(c, this);
        if (stashed > 0 && Owner.Creature.GetPower<RaccoonStashStatePower>()?.Count == 1)
            await CardPileCmd.Draw(c, IsUpgraded? 2 : 1, Owner);
    }
    protected override void OnUpgrade()
    {
    }
}
