using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class ConstellationNapPower : FootprintListenerPower
{ private int _turn = -1; public override PowerType Type => PowerType.Buff; public override PowerStackType StackType => PowerStackType.Single; public override async Task OnEmptySlotFilled(PlayerChoiceContext c, CardPlay p) { int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1; if (turn == _turn) return; _turn = turn; await PlayerCmd.GainEnergy(1, Owner.Player!); if (Amount > 1) await CardPileCmd.Draw(c, 1, Owner.Player!); } }
