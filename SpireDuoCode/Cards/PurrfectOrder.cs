using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class PurrfectOrder() : BlackCatCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{ protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PurrfectOrderPower>(6m)]; protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PurrfectOrderPower>()]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<PurrfectOrderPower>(c, Owner.Creature, DynamicVars["PurrfectOrderPower"].BaseValue, Owner.Creature, this); protected override void OnUpgrade() => DynamicVars["PurrfectOrderPower"].UpgradeValueBy(3m); }
