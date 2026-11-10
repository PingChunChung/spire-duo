using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Catalogue() : RaccoonCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CataloguePower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<CataloguePower>()];
    protected override Task OnPlay(PlayerChoiceContext c, CardPlay p) => PowerCmd.Apply<CataloguePower>(c, Owner.Creature, DynamicVars["CataloguePower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(- 1);
}
