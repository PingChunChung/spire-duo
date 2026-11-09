using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class MoonlitRelay() : BlackCatCard(1, CardType.Skill, CardRarity.Rare, TargetType.AnyAlly)
{ public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly; public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { if (p.Target?.Player == null) return; await PlayerCmd.GainEnergy(1, p.Target.Player); await CardPileCmd.Draw(c, IsUpgraded ? 3 : 2, p.Target.Player); } protected override void OnUpgrade() { } }
