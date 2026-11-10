using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class FalseBottom() : RaccoonCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await RaccoonCardEffects.Stash(c, this, 2);
        await PowerCmd.Apply<DrawCardsNextTurnPower>(c, Owner.Creature, IsUpgraded? 3 : 2, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
    }
}
