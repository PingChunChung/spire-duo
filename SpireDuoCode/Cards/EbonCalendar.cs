using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class EbonCalendar() : BlackCatCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{ protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<EbonCalendarPower>(1m)]; protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<EbonCalendarPower>()]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<EbonCalendarPower>(c, Owner.Creature, DynamicVars["EbonCalendarPower"].BaseValue, Owner.Creature, this); protected override void OnUpgrade() => DynamicVars["EbonCalendarPower"].UpgradeValueBy(1m); }
