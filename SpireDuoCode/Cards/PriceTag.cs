using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class PriceTag() : RaccoonCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PriceTagPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PriceTagPower>()];
    protected override Task OnPlay(PlayerChoiceContext c, CardPlay p) => PowerCmd.Apply<PriceTagPower>(c, Owner.Creature, DynamicVars["PriceTagPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["PriceTagPower"].UpgradeValueBy(1m);
}
