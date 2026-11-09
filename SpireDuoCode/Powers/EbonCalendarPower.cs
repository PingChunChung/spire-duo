using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class EbonCalendarPower : FootprintListenerPower
{ private int _turn = -1; public override PowerType Type => PowerType.Buff; public override PowerStackType StackType => PowerStackType.Single; public override async Task OnFootprintLine(PlayerChoiceContext c, CardPlay p, FootprintLine l) { int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1; if (l != FootprintLine.Omen || turn == _turn) return; _turn = turn; var card = (await CardSelectCmd.FromCombatPile(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1), context: c, pile: PileType.Discard.GetPile(Owner.Player!), player: Owner.Player!)).FirstOrDefault(); if (card != null) { if (Amount > 1) card.EnergyCost.SetUntilPlayed(0, true); await CardPileCmd.Add(card, PileType.Hand); } } }
