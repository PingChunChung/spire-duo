using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class InkwellSky() : BlackCatCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{ protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<InkwellSkyPower>(5m)]; protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<InkwellSkyPower>()]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<InkwellSkyPower>(c, Owner.Creature, DynamicVars["InkwellSkyPower"].BaseValue, Owner.Creature, this); protected override void OnUpgrade() => DynamicVars["InkwellSkyPower"].UpgradeValueBy(2m); }
