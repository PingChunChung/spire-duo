using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(EventRelicPool))]
public sealed class EclipseChime : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override Task BeforeCombatStart()
    {
        Flash();
        // TODO(cat-batch-2): set A-S, then lock the middle Footprint slot through the first turn.
        return Task.CompletedTask;
    }
}
