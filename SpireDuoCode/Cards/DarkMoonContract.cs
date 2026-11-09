using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DarkMoonContract() : BlackCatCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{ protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DarkMoonContractPower>(IsUpgraded ? 1m : 0m)]; protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DarkMoonContractPower>()]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<DarkMoonContractPower>(c, Owner.Creature, DynamicVars["DarkMoonContractPower"].BaseValue, Owner.Creature, this); protected override void OnUpgrade() => DynamicVars["DarkMoonContractPower"].UpgradeValueBy(1m); }
