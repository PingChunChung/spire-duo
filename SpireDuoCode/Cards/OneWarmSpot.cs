using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class OneWarmSpot() : BlackCatCard(2, CardType.Skill, CardRarity.Rare, TargetType.AnyAlly)
{ public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { if (p.Target?.Player != null) { foreach (var card in PileType.Hand.GetPile(p.Target.Player).Cards) card.GiveSingleTurnRetain(); if (IsUpgraded) await CardPileCmd.Draw(c, 2, p.Target.Player); } await FootprintStatePower.LockAllNonEmpty(c, Owner.Creature, this); } protected override void OnUpgrade() { } }
