using HarmonyLib;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Runs;
using SpireDuo.SpireDuoCode.Character;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Patches;

/// <summary>Installs the hidden footprint listener before vanilla dispatches relic combat-start hooks.</summary>
[HarmonyPatch(typeof(Hook), nameof(Hook.BeforeCombatStart))]
public static class FootprintCombatStartPatch
{
    [HarmonyPrefix]
    private static bool BeforeCombatStart(IRunState runState, ICombatState? combatState, ref Task __result)
    {
        __result = BeforeCombatStartWithFootprints(runState, combatState);
        return false;
    }

    private static async Task BeforeCombatStartWithFootprints(IRunState runState, ICombatState? combatState)
    {
        if (combatState != null)
        {
            foreach (var player in combatState.Players.Where(player => player.Character is BlackCat))
                await FootprintStatePower.GetOrCreate(new ThrowingPlayerChoiceContext(), player.Creature);
            foreach (var player in combatState.Players.Where(player => player.Character is Raccoon))
                await RaccoonStashStatePower.GetOrCreate(new ThrowingPlayerChoiceContext(), player.Creature);
        }

        foreach (var model in runState.IterateHookListeners(combatState))
        {
            await model.BeforeCombatStart();
            model.InvokeExecutionFinished();
        }
        foreach (var model in runState.IterateHookListeners(combatState))
        {
            await model.BeforeCombatStartLate();
            model.InvokeExecutionFinished();
        }
    }
}
