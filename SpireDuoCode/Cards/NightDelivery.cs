using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class NightDelivery() : RaccoonCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        if (p.Target?.Player != null && await RaccoonStashStatePower.Spend(c, Owner.Creature, this)
            != null)
        {
            await PlayerCmd.GainEnergy(1, p.Target.Player);
            if (IsUpgraded)
                await CardPileCmd.Draw(c, 1, p.Target.Player);
        }
    }
    protected override void OnUpgrade()
    {
    }
}
