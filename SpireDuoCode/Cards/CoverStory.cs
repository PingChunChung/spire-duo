using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class CoverStory() : RaccoonCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CoverStoryPower>(5m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<CoverStoryPower>()];
    protected override Task OnPlay(PlayerChoiceContext c, CardPlay p) => PowerCmd.Apply<CoverStoryPower>(c, Owner.Creature, DynamicVars["CoverStoryPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["CoverStoryPower"].UpgradeValueBy(2m);
}
