using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class CutIn() : RaccoonCard(1, CardType.Skill, CardRarity.Rare, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        if (p.Target?.Player == null || await RaccoonStashStatePower.Spend(c, Owner.Creature, this, x => RaccoonStashStatePower.CostValue(x) >= 2) == null)
            return;
        await PlayerCmd.GainEnergy(2, p.Target.Player);
        if (IsUpgraded)
            await CardPileCmd.Draw(c, 1, p.Target.Player);
    }
    protected override void OnUpgrade()
    {
    }
}
