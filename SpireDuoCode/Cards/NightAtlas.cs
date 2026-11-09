using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class NightAtlas() : BlackCatCard(3, CardType.Power, CardRarity.Ancient, TargetType.Self)
{ protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<NightAtlasPower>(IsUpgraded ? 4m : 0m)]; protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<NightAtlasPower>()]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<NightAtlasPower>(c, Owner.Creature, DynamicVars["NightAtlasPower"].BaseValue, Owner.Creature, this); protected override void OnUpgrade() { } }
