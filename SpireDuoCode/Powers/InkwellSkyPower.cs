using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class InkwellSkyPower : SpireDuoPower
{ public override PowerType Type => PowerType.Buff; public override PowerStackType StackType => PowerStackType.Counter; public override async Task AfterCardPlayed(PlayerChoiceContext c, CardPlay p) { if (p.Card.Owner != Owner.Player || p.Card.Type != CardType.Power) return; await CreatureCmd.Damage(c, Owner.CombatState!.HittableEnemies, Amount, ValueProp.Unpowered, Owner, p.Card); } }
