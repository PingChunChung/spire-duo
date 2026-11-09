using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class ConstellationNap() : BlackCatCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{ protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ConstellationNapPower>(IsUpgraded ? 2m : 1m)]; protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ConstellationNapPower>()]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<ConstellationNapPower>(c, Owner.Creature, DynamicVars["ConstellationNapPower"].BaseValue, Owner.Creature, this); protected override void OnUpgrade() { } }
