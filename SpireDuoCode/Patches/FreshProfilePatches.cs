using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;

namespace SpireDuo.SpireDuoCode.Patches;

/// <summary>
/// On a profile that has not revealed the Neow epoch yet (a fresh modded profile), the first act has no
/// Ancient at all, because Neow is the only Ancient of Overgrowth and Underdocks. Keep Neow available.
/// </summary>
[HarmonyPatch]
public static class AlwaysNeowPatch
{
    [HarmonyPatch(typeof(Overgrowth), nameof(Overgrowth.GetUnlockedAncients))]
    [HarmonyPostfix]
    public static void OvergrowthPostfix(ref IEnumerable<AncientEventModel> __result) => AddNeow(ref __result);

    [HarmonyPatch(typeof(Underdocks), nameof(Underdocks.GetUnlockedAncients))]
    [HarmonyPostfix]
    public static void UnderdocksPostfix(ref IEnumerable<AncientEventModel> __result) => AddNeow(ref __result);

    private static void AddNeow(ref IEnumerable<AncientEventModel> result)
    {
        var neow = ModelDb.AncientEvent<Neow>();
        var list = result.ToList();
        if (list.Contains(neow)) return;
        list.Add(neow);
        result = list;
    }
}

/// <summary>
/// Without the Neow epoch the run also skips the Ancient room: act 1's starting point becomes a Monster room.
/// Always start at the Ancient room, together with <see cref="AlwaysNeowPatch"/>.
/// </summary>
[HarmonyPatch(typeof(RunManager), "SetStartedWithNeowFlag")]
public static class StartWithNeowPatch
{
    private static readonly PropertyInfo StateProperty = AccessTools.Property(typeof(RunManager), "State");

    public static void Postfix(RunManager __instance)
    {
        if (StateProperty.GetValue(__instance) is RunState state)
            state.ExtraFields.StartedWithNeow = true;
    }
}

/// <summary>
/// The first chest of a player's first run forces Gorget. In co-op every player gets that forced relic, so the
/// chest shows two identical Gorgets. Only the first player keeps the tutorial relic; the rest roll normally.
/// </summary>
[HarmonyPatch(typeof(TreasureRoomRelicSynchronizer), "TryGetRelicForTutorial")]
public static class TutorialChestPatch
{
    public static void Postfix(ref RelicModel? __result, List<RelicModel>? ____currentRelics)
    {
        if (__result is Gorget && ____currentRelics != null && ____currentRelics.Any(r => r is Gorget))
            __result = null;
    }
}
