using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class CatOfTheThreshold() : BlackCatCard(2, CardType.Power, CardRarity.Ancient, TargetType.Self)
{ protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CatOfTheThresholdPower>(IsUpgraded ? 12m : 0m)]; protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<CatOfTheThresholdPower>()]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<CatOfTheThresholdPower>(c, Owner.Creature, DynamicVars["CatOfTheThresholdPower"].BaseValue, Owner.Creature, this); protected override void OnUpgrade() { } }
