using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Rooms;
using SpireDuo.SpireDuoCode.Character;
using SpireDuo.SpireDuoCode.Progress;

namespace SpireDuo.SpireDuoCode.Patches;

/// <summary>Records only local keepsakes. These hooks run in every deterministic client simulation.</summary>
[HarmonyPatch]
public static class MilestoneRunPatches
{
    private static RunState? _endedRun;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(RunManager), "InitializeNewRun")]
    private static void RecordNewRun(RunManager __instance)
    {
        RunState? state = AccessTools.Property(typeof(RunManager), "State")?.GetValue(__instance) as RunState;
        if (state == null || !MilestoneTracker.IsModRun(state))
            return;

        MilestoneTracker.BeginRun();
        ProgressStore.IncrementStat(stats => stats.Runs, (stats, value) => stats.Runs = value);
        MilestoneTracker.Unlock(MilestoneIds.FirstSteps);
        if (ProgressStore.Current.Stats.Runs >= 10)
            MilestoneTracker.Unlock(MilestoneIds.Runs10);
        if (MilestoneTracker.IsCoopModRun(state))
        {
            ProgressStore.IncrementStat(stats => stats.CoopRuns, (stats, value) => stats.CoopRuns = value);
            MilestoneTracker.Unlock(MilestoneIds.SideBySide);
        }
        if (state.Players.Any(player => player.Character is BlackCat))
            MilestoneTracker.Unlock(MilestoneIds.CatFirstRun);
        ProgressStore.Flush();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CombatRoom), nameof(CombatRoom.OnCombatEnded))]
    private static void RecordCombatVictory(CombatRoom __instance)
    {
        IRunState state = __instance.CombatState.RunState;
        if (!MilestoneTracker.IsModRun(state))
            return;

        ProgressStore.IncrementStat(stats => stats.CombatsWon, (stats, value) => stats.CombatsWon = value);
        MilestoneTracker.Unlock(MilestoneIds.FirstWin);
        if (__instance.RoomType == RoomType.Elite)
        {
            ProgressStore.IncrementStat(stats => stats.EliteWins, (stats, value) => stats.EliteWins = value);
            MilestoneTracker.Unlock(MilestoneIds.EliteWin);
        }
        if (MilestoneTracker.ConsumeCombatFall(__instance.CombatState))
            MilestoneTracker.Unlock(MilestoneIds.BackOnFeet);
        MilestoneTracker.CompleteCombat(__instance.CombatState);
        if (__instance.RoomType == RoomType.Boss)
        {
            if (state.CurrentActIndex == 0)
                MilestoneTracker.Unlock(MilestoneIds.Act1Clear);
            else if (state.CurrentActIndex == 1)
                MilestoneTracker.Unlock(MilestoneIds.Act2Clear);
        }
        ProgressStore.Flush();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Creature), "SetCurrentHpInternal")]
    private static void RecordPlayerFall(Creature __instance)
    {
        if (__instance.IsDead)
            MilestoneTracker.RecordPlayerFall(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(RunManager), nameof(RunManager.OnEnded))]
    private static void RecordRunEnd(RunManager __instance, bool isVictory)
    {
        RunState? state = AccessTools.Property(typeof(RunManager), "State")?.GetValue(__instance) as RunState;
        if (state == null || !MilestoneTracker.IsModRun(state))
            return;
        // WinRun calls OnEnded(true) and then kills every player for the end screen, which calls OnEnded(false) again.
        if (ReferenceEquals(state, _endedRun))
            return;
        _endedRun = state;

        if (!isVictory)
        {
            MilestoneTracker.RecordRunEnd(lost: true);
            ProgressStore.IncrementStat(stats => stats.Defeats, (stats, value) => stats.Defeats = value);
            ProgressStore.Flush();
            return;
        }

        MilestoneTracker.RecordRunEnd(lost: false);
        ProgressStore.IncrementStat(stats => stats.RunWins, (stats, value) => stats.RunWins = value);
        if (ProgressStore.Current.Stats.RunWins >= 5)
            MilestoneTracker.Unlock(MilestoneIds.Wins5);
        if (state.CurrentActIndex >= 2)
        {
            MilestoneTracker.Unlock(MilestoneIds.SpireClear);
            if (state.Players.Any(player => player.Character is Koala))
                MilestoneTracker.Unlock(MilestoneIds.KoalaClear);
            if (state.Players.Any(player => player.Character is Panda))
                MilestoneTracker.Unlock(MilestoneIds.PandaClear);
            if (state.Players.Any(player => player.Character is BlackCat))
                MilestoneTracker.Unlock(MilestoneIds.CatVictory);
            if (MilestoneTracker.IsCoopModRun(state))
                MilestoneTracker.Unlock(MilestoneIds.TwoPersonClear);
        }
        ProgressStore.Flush();
    }
}
