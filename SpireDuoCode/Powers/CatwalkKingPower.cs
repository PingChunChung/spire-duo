using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class CatwalkKingPower : FootprintListenerPower
{ private int _turn = -1; public override PowerType Type => PowerType.Buff; public override PowerStackType StackType => PowerStackType.Single; public override async Task OnFootprintLine(PlayerChoiceContext c, CardPlay p, FootprintLine line) { int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1; if (line != FootprintLine.Pounce || turn == _turn) return; _turn = turn; var copy = p.Card.CreateClone(); copy.EnergyCost.SetThisTurn(0); copy.AddKeyword(CardKeyword.Exhaust); Flash(); await CardPileCmd.AddGeneratedCardToCombat(copy, PileType.Hand, Owner.Player!); } }
