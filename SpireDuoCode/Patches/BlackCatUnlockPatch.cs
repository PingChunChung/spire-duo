using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Runs;
using SpireDuo.SpireDuoCode.Character;
using SpireDuo.SpireDuoCode.Progress;

namespace SpireDuo.SpireDuoCode.Patches;

/// <summary>
/// Beating Act 3 with the Koala and the Panda in one party unlocks the Black Cat. Every client runs OnEnded,
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
        if (!state.Players.Any(p => p.Character is Koala) || !state.Players.Any(p => p.Character is Panda))
            return;
        if (ProgressStore.Current.BlackCatUnlocked)
            return;

        ProgressStore.UnlockBlackCat();
        MainFile.Logger.Info("Black Cat unlocked");
        ShowBanner();
    }

    private static void ShowBanner()
    {
        if (Engine.GetMainLoop() is not SceneTree tree)
            return;

        var layer = new CanvasLayer { Layer = 100 };
        var label = new Label
        {
            Text = new LocString("characters", "SPIREDUO-BLACK_CAT.unlockedBanner").GetFormattedText(),
            HorizontalAlignment = HorizontalAlignment.Center,
            AnchorLeft = 0f,
            AnchorRight = 1f,
            OffsetTop = 120f,
            MouseFilter = Control.MouseFilterEnum.Ignore
        };
        label.AddThemeFontSizeOverride("font_size", 36);
        label.AddThemeColorOverride("font_color", new Color(1f, 0.86f, 0.45f));
        label.AddThemeColorOverride("font_outline_color", Colors.Black);
        label.AddThemeConstantOverride("outline_size", 8);
        layer.AddChild(label);
        tree.Root.CallDeferred(Node.MethodName.AddChild, layer);
        tree.CreateTimer(6.0).Timeout += layer.QueueFree;
    }
}
