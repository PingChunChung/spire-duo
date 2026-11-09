using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;
using SpireDuo.SpireDuoCode.Relics;

namespace SpireDuo.SpireDuoCode.Patches;

/// <summary>
/// Touch of Orobas only knows the vanilla starter relics and turns any other starter relic into Circlet, so picking
/// it as Koala or Panda would lose the starter relic. Map ours to their upgraded versions.
/// </summary>
[HarmonyPatch(typeof(TouchOfOrobas), nameof(TouchOfOrobas.GetUpgradedStarterRelic))]
public static class TouchOfOrobasPatch
{
    public static bool Prefix(RelicModel starterRelic, ref RelicModel __result)
    {
        RelicModel? upgraded = starterRelic switch
        {
            EucalyptusLeaf => ModelDb.Relic<EucalyptusCrown>(),
            BambooBasket => ModelDb.Relic<BambooHamper>(),
            MoonbellCollar => ModelDb.Relic<EclipseChime>(),
            _ => null
        };
        if (upgraded == null) return true;
        __result = upgraded;
        return false;
    }
}
