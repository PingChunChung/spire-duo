using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class FelineVelocityPower : SpireDuoPower
{ private int _attacks; public override PowerType Type => PowerType.Buff; public override PowerStackType StackType => PowerStackType.Single; public override async Task AfterCardPlayed(PlayerChoiceContext c, CardPlay p) { if (p.Card.Owner != Owner.Player || p.Card.Type != CardType.Attack || ++_attacks < Amount) return; _attacks = 0; Flash(); await CardPileCmd.Draw(c, 1, Owner.Player!); await PlayerCmd.GainEnergy(1, Owner.Player!); } }
