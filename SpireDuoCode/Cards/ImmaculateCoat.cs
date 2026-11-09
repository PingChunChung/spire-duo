using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class ImmaculateCoat() : BlackCatCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{ protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ImmaculateCoatPower>(5m)]; protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ImmaculateCoatPower>()]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<ImmaculateCoatPower>(c, Owner.Creature, DynamicVars["ImmaculateCoatPower"].BaseValue, Owner.Creature, this); protected override void OnUpgrade() => DynamicVars["ImmaculateCoatPower"].UpgradeValueBy(2m); }
