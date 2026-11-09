using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class QuietPactPower : FootprintListenerPower
{ public override PowerType Type => PowerType.Buff; public override PowerStackType StackType => PowerStackType.Single; public override async Task OnCompanionResolved(PlayerChoiceContext c, CardPlay p, Creature? ally) { await PowerCmd.Apply<ArtifactPower>(c, Owner, 1, Owner, p.Card); if (ally != null && !ally.IsDead) await PowerCmd.Apply<ArtifactPower>(c, ally, 1, Owner, p.Card); if (Amount > 1) await CardPileCmd.Draw(c, 1, Owner.Player!); } }
