using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SharedStash() : RaccoonCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        if (p.Target?.Player != null)
        {
            await CardPileCmd.Draw(c, 2, p.Target.Player);
            if (IsUpgraded)
                await CreatureCmd.GainBlock(p.Target, 5m, ValueProp.Move, p);
        }
        await RaccoonCardEffects.Stash(c, this);
    }
    protected override void OnUpgrade()
    {
    }
}
