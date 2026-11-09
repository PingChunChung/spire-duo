using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(BlackCatRelicPool))]
public sealed class MoonbellCollar : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override Task BeforeCombatStart()
    {
        Flash();
        // TODO(cat-batch-2): set the middle Footprint slot to S.
        return Task.CompletedTask;
    }
}
