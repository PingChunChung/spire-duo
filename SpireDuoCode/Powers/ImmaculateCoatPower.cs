using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class ImmaculateCoatPower : FootprintListenerPower
{ public override PowerType Type => PowerType.Buff; public override PowerStackType StackType => PowerStackType.Counter; public override async Task OnFootprintLine(PlayerChoiceContext c, CardPlay p, FootprintLine l) { if (l != FootprintLine.Groom) return; Flash(); await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, p); } }
