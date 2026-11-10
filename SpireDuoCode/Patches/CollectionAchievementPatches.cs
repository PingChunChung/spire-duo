using MegaCrit.Sts2.Core.Entities.Relics;
using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using SpireDuo.SpireDuoCode.Events;
using SpireDuo.SpireDuoCode.Progress;
using SpireDuo.SpireDuoCode.Relics;

namespace SpireDuo.SpireDuoCode.Patches;

[HarmonyPatch]
public static class CollectionAchievementPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.AddRelicInternal))]
    private static void RecordRelic(Player __instance, RelicModel relic)
    {
        // Starting relics come with the character, so they do not count as finds.
        if (relic is SpireDuoRelic && relic.Rarity != RelicRarity.Starter)
            MilestoneTracker.RecordRelicCollected(relic.Id.Entry, __instance.RunState);
    }

}

[HarmonyPatch]
public static class EventCollectionAchievementPatch
{
    [HarmonyTargetMethod]
    private static MethodBase TargetMethod() => AccessTools.Method(typeof(EventModel), "SetEventFinished")!;

    [HarmonyPostfix]
    private static void RecordEvent(EventModel __instance)
    {
        Player? owner = AccessTools.Property(typeof(EventModel), "Owner")?.GetValue(__instance) as Player;
        if (__instance is SpireDuoEvent && owner?.RunState is { } state)
            MilestoneTracker.RecordEventFinished(__instance.Id.Entry, state);
    }
}
