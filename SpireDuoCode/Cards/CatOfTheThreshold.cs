using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class CatOfTheThreshold() : BlackCatCard(2, CardType.Power, CardRarity.Ancient, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CatOfTheThresholdPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<CatOfTheThresholdPower>()];

    protected override Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay) =>
        PowerCmd.Apply<CatOfTheThresholdPower>(context, Owner.Creature, DynamicVars["CatOfTheThresholdPower"].BaseValue, Owner.Creature, this);

    protected override void OnUpgrade() => DynamicVars["CatOfTheThresholdPower"].UpgradeValueBy(12m);
}
