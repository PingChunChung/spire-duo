using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class SecretSignalPower : SpireDuoPower
{ public override PowerType Type => PowerType.Buff; public override PowerStackType StackType => PowerStackType.Single; public override async Task AfterCardPlayed(PlayerChoiceContext c, CardPlay p) { var state = Owner.GetPower<FootprintStatePower>(); if (state?.CompanionArmed != true || p.Card.Type != CardType.Attack || p.Card.Owner.Creature.Side != Owner.Side) return; await CardPileCmd.Draw(c, Amount, Owner.Player!); } }
