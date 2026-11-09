using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SharedNightshift() : BlackCatCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{ protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SharedNightshiftPower>(IsUpgraded ? 7m : 5m)]; protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<SharedNightshiftPower>()]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<SharedNightshiftPower>(c, Owner.Creature, DynamicVars["SharedNightshiftPower"].BaseValue, Owner.Creature, this); protected override void OnUpgrade() { } }
