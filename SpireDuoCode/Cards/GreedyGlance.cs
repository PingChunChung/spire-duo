using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class GreedyGlance() : RaccoonCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<GreedyGlancePower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<GreedyGlancePower>()];
    protected override Task OnPlay(PlayerChoiceContext c, CardPlay p) => PowerCmd.Apply<GreedyGlancePower>(c, Owner.Creature, DynamicVars["GreedyGlancePower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["GreedyGlancePower"].UpgradeValueBy(1m);
}
