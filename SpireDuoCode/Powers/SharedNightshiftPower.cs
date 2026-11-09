using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class SharedNightshiftPower : SpireDuoPower
{ private int _turn = -1; public override PowerType Type => PowerType.Buff; public override PowerStackType StackType => PowerStackType.Counter; public override async Task AfterCardPlayed(PlayerChoiceContext c, CardPlay p) { int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1; if (turn == _turn || p.Card.Owner == Owner.Player || p.Card.Owner.Creature.Side != Owner.Side || p.Card.Type != CardType.Attack) return; _turn = turn; await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, p); await CreatureCmd.GainBlock(p.Card.Owner.Creature, Amount - 2, ValueProp.Unpowered, p); } }
