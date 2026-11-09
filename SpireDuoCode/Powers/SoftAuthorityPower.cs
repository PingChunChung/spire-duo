using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class SoftAuthorityPower : FootprintListenerPower
{ private int _turn = -1; public override PowerType Type => PowerType.Buff; public override PowerStackType StackType => PowerStackType.Single; public override async Task OnFootprintLine(PlayerChoiceContext c, CardPlay p, FootprintLine l) { int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1; if (l != FootprintLine.Groom || turn == _turn) return; _turn = turn; foreach (var power in Owner.Powers.Where(x => x.Type == PowerType.Debuff).ToArray()) await PowerCmd.Remove(power); if (Amount > 1) await CardPileCmd.Draw(c, 1, Owner.Player!); } }
