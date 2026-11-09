using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SecretSignal() : BlackCatCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{ protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SecretSignalPower>(2m)]; protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<SecretSignalPower>()]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<SecretSignalPower>(c, Owner.Creature, DynamicVars["SecretSignalPower"].BaseValue, Owner.Creature, this); protected override void OnUpgrade() => DynamicVars["SecretSignalPower"].UpgradeValueBy(1m); }
