using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class GuardianOfTheEaves() : BlackCatCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{ protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<GuardianOfTheEavesPower>(IsUpgraded ? 12m : 8m)]; protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<GuardianOfTheEavesPower>()]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<GuardianOfTheEavesPower>(c, Owner.Creature, DynamicVars["GuardianOfTheEavesPower"].BaseValue, Owner.Creature, this); protected override void OnUpgrade() { } }
