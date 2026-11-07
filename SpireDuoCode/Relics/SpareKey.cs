using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(SharedRelicPool))]
public sealed class SpareKey : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(10m)];

    public override bool IsAllowed(IRunState runState) => runState.Players.Count > 1;

    public override async Task AfterRestSiteHeal(Player player, bool isMimicked)
    {
        if (player != Owner) return;
        Flash();
        foreach (var other in Owner.RunState.Players.Where(p => p != Owner && p.Creature.IsAlive).ToList())
        {
            await CreatureCmd.Heal(other.Creature, DynamicVars.Heal.BaseValue);
        }
    }
}
