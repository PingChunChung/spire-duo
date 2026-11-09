using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class PurrfectOrderPower : SpireDuoPower
{ private int _turn = -1; private bool _ready; public override PowerType Type => PowerType.Buff; public override PowerStackType StackType => PowerStackType.Counter; public override async Task AfterCardPlayed(PlayerChoiceContext c, CardPlay p) { if (p.Card.Owner != Owner.Player) return; int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1; if (p.Card.Type == CardType.Skill && turn != _turn) { _turn = turn; _ready = true; } else if (p.Card.Type == CardType.Attack) _ready = false; await Task.CompletedTask; } public override decimal ModifyDamageAdditive(Creature? t, decimal a, ValueProp v, Creature? d, CardModel? card) => _ready && d == Owner && v.IsPoweredAttack() ? Amount : 0m; }
