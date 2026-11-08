using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Combat;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Patches;

/// <summary>
/// The game plays a Spine "Dead" animation when a player dies and has no fallback for picture-only characters, so a
/// dead Koala or Panda kept standing. Tip them over and grey them out instead, and stand them back up on revive
/// (co-op revives dead players when the fight is won).
/// </summary>
[HarmonyPatch]
public static class PlayerDeathAnimPatch
{
    private const float FallenRotation = -1.45f;
    private static readonly Color FallenTint = new(0.55f, 0.55f, 0.6f);

    [HarmonyPatch(typeof(NCreature), nameof(NCreature.StartDeathAnim))]
    [HarmonyPostfix]
    public static void DeathPostfix(NCreature __instance)
    {
        if (!IsOurs(__instance)) return;
        var tween = __instance.CreateTween().SetParallel();
        tween.TweenProperty(__instance.Visuals, "rotation", FallenRotation, 0.45).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quad);
        tween.TweenProperty(__instance.Visuals, "modulate", FallenTint, 0.45);
    }

    [HarmonyPatch(typeof(NCreature), nameof(NCreature.StartReviveAnim))]
    [HarmonyPostfix]
    public static void RevivePostfix(NCreature __instance)
    {
        if (!IsOurs(__instance)) return;
        var tween = __instance.CreateTween().SetParallel();
        tween.TweenProperty(__instance.Visuals, "rotation", 0f, 0.35).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
        tween.TweenProperty(__instance.Visuals, "modulate", Colors.White, 0.35);
    }

    private static bool IsOurs(NCreature creature) =>
        !creature.HasSpineAnimation && creature.Entity.Player?.Character is Koala or Panda;
}
