using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class DarkMoonContractPower : SpireDuoPower
{ public override PowerType Type => PowerType.Buff; public override PowerStackType StackType => PowerStackType.Single; public override async Task AfterPlayerTurnStart(PlayerChoiceContext c, Player p) { if (p != Owner.Player) return; await FootprintStatePower.Set(c, Owner, 0, Footprint.Power); await CardPileCmd.Draw(c, 1, p); } }
