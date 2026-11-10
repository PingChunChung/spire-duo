using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Runs;
using SpireDuo.SpireDuoCode.Character;
using SpireDuo.SpireDuoCode.Progress;

namespace SpireDuo.SpireDuoCode.Patches;

/// <summary>
/// Beating Act 3 with the Koala and the Panda in one party unlocks the Black Cat; so does beating it once with each of them,
/// which keeps the unlock reachable in singleplayer. Every client runs OnEnded,
/// so each player writes their own mod progress file; the game's save is never touched.
/// </summary>
[HarmonyPatch(typeof(RunManager), nameof(RunManager.OnEnded))]
public static class BlackCatUnlockPatch
{
    private static readonly AccessTools.FieldRef<RunManager, RunState?> State =
        AccessTools.FieldRefAccess<RunManager, RunState?>("<State>k__BackingField");

    [HarmonyPostfix]
    private static void UnlockOnVictory(RunManager __instance, bool isVictory)
    {
        RunState? state = State(__instance);
        if (!isVictory || state == null || state.CurrentActIndex < 2)
            return;
        bool koala = state.Players.Any(p => p.Character is Koala);
        bool panda = state.Players.Any(p => p.Character is Panda);
        if (!koala && !panda)
            return;

        SanaCkoalaProgress progress = ProgressStore.RecordSpireClear(koala, panda);
        if (progress.BlackCatUnlocked || !progress.KoalaSpireCleared || !progress.PandaSpireCleared)
            return;

        ProgressStore.UnlockBlackCat();
        MainFile.Logger.Info("Black Cat unlocked");
        MilestoneBanner.EnqueueMessage(new LocString("characters", "SPIREDUO-BLACK_CAT.unlockedBanner").GetFormattedText());
        MilestoneTracker.Unlock(MilestoneIds.BlackCatJoins);
    }
}
