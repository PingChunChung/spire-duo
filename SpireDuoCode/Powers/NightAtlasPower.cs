using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class NightAtlasPower : FootprintListenerPower
{ private int _turn = -1; public override PowerType Type => PowerType.Buff; public override PowerStackType StackType => PowerStackType.Single; public override async Task OnFootprintLine(PlayerChoiceContext c, CardPlay p, FootprintLine l) { int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1; if (turn == _turn) return; _turn = turn; await PlayerCmd.GainEnergy(1, Owner.Player!); await CardPileCmd.Draw(c, 1, Owner.Player!); if (Amount > 0) await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, p); } }
