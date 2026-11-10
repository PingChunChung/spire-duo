using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class NightShift() : RaccoonCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<NightShiftPower>(6m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<NightShiftPower>()];
    protected override Task OnPlay(PlayerChoiceContext c, CardPlay p) => PowerCmd.Apply<NightShiftPower>(c, Owner.Creature, DynamicVars["NightShiftPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["NightShiftPower"].UpgradeValueBy(2m);
}
