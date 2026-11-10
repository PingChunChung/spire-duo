using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;
using MegaCrit.Sts2.Core.Runs;
using SpireDuo.SpireDuoCode.Progress;

namespace SpireDuo.SpireDuoCode.Patches;

[HarmonyPatch(typeof(NGameOverScreen), nameof(NGameOverScreen._Ready))]
public static class EndRunAchievementPanelPatch
{
    private const string PanelName = "SanaCkoalaEndRunAchievements";

    [HarmonyPostfix]
    private static void AddPanel(NGameOverScreen __instance)
    {
        RunState? state = AccessTools.Property(typeof(RunManager), "State")?.GetValue(RunManager.Instance) as RunState;
        if (state == null || !MilestoneTracker.IsModRun(state) || __instance.GetNodeOrNull(PanelName) != null)
            return;

        var panel = new PanelContainer
        {
            Name = PanelName,
            AnchorLeft = 0.68f,
            AnchorTop = 0.50f,
            AnchorRight = 0.95f,
            AnchorBottom = 0.86f,
            GrowHorizontal = Control.GrowDirection.Both,
            GrowVertical = Control.GrowDirection.Both,
            MouseFilter = Control.MouseFilterEnum.Ignore
        };
        panel.AddThemeStyleboxOverride("panel", new StyleBoxFlat
        {
            BgColor = new Color(0.06f, 0.04f, 0.09f, 0.86f),
            BorderColor = new Color(0.85f, 0.68f, 0.34f, 0.8f),
            BorderWidthLeft = 2,
            BorderWidthTop = 2,
            BorderWidthRight = 2,
            BorderWidthBottom = 2,
            CornerRadiusTopLeft = 8,
            CornerRadiusTopRight = 8,
            CornerRadiusBottomLeft = 8,
            CornerRadiusBottomRight = 8,
            ContentMarginLeft = 18,
            ContentMarginTop = 14,
            ContentMarginRight = 18,
            ContentMarginBottom = 14
        });
        var lines = new VBoxContainer { MouseFilter = Control.MouseFilterEnum.Ignore };
        panel.AddChild(lines);
        if (MilestoneTracker.GetRunUnlocks().Any(definition => definition.Id == MilestoneIds.BlackCatJoins))
            AddBlackCatReveal(lines);
        // The Black Cat already has its own reveal above; list the rarest of the rest first.
        List<AchievementDefinition> earned = MilestoneTracker.GetRunUnlocks()
            .Where(definition => definition.Id != MilestoneIds.BlackCatJoins)
            .OrderByDescending(definition => definition.Tier)
            .Take(3)
            .ToList();
        if (earned.Count > 0)
        {
            AddHeading(lines, "ALBUM.runEarned");
            foreach (AchievementDefinition definition in earned)
                AddLine(lines, Text($"{definition.Id}.title"));
        }

        List<AchievementDefinition> progressing = AchievementCatalog.All
            .Where(definition => !definition.Hidden && definition.ProgressTarget > 0 && !ProgressStore.Current.Achievements.ContainsKey(definition.Id))
            .Where(definition => MilestoneTracker.GetStat(definition.StatKey!) > 0)
            .Where(definition => definition.Page != AchievementPage.BlackCat || ProgressStore.Current.BlackCatUnlocked)
            .Where(definition => definition.Page != AchievementPage.Raccoon || ProgressStore.Current.RaccoonUnlocked)
            .OrderByDescending(definition => (float)MilestoneTracker.GetStat(definition.StatKey!) / definition.ProgressTarget)
            .Take(3)
            .ToList();
        if (progressing.Count > 0)
        {
            AddHeading(lines, "ALBUM.runProgressing");
            foreach (AchievementDefinition definition in progressing)
                AddLine(lines, $"{Text($"{definition.Id}.title")}  {Math.Min(definition.ProgressTarget, MilestoneTracker.GetStat(definition.StatKey!))} / {definition.ProgressTarget}");
        }
        if (MilestoneTracker.LastRunWasLost)
        {
            // The old "Next time" milestone picture now closes out a lost run instead of being an achievement.
            const string tryAgainPicture = "res://SanaCkoala/images/milestones/see_you_next_time.png";
            if (ResourceLoader.Exists(tryAgainPicture))
            {
                lines.AddChild(new TextureRect
                {
                    Texture = ResourceLoader.Load<Texture2D>(tryAgainPicture),
                    CustomMinimumSize = new Vector2(240f, 160f),
                    ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                    StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                    SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter,
                    MouseFilter = Control.MouseFilterEnum.Ignore
                });
            }
            AddLine(lines, Text("ALBUM.runTryAgain"));
        }

        if (lines.GetChildCount() == 0)
        {
            panel.QueueFree();
            return;
        }
        __instance.AddChild(panel);
    }

    private static void AddBlackCatReveal(Container parent)
    {
        var reveal = new VBoxContainer { MouseFilter = Control.MouseFilterEnum.Ignore };
        reveal.AddThemeConstantOverride("separation", 6);
        parent.AddChild(reveal);
        TextureRect? picture = AlbumVisuals.CreateOptionalTexture("res://SanaCkoala/images/milestones/black_cat_joins.png", new Vector2(220, 145));
        if (picture != null)
        {
            picture.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
            reveal.AddChild(picture);
        }
        AddHeading(reveal, "ALBUM.catRevealTitle");
        AddLine(reveal, Text("ALBUM.catRevealBody"));
    }

    private static void AddHeading(Container parent, string key)
    {
        var label = new Label { Text = Text(key), MouseFilter = Control.MouseFilterEnum.Ignore };
        label.AddThemeFontSizeOverride("font_size", 22);
        label.AddThemeColorOverride("font_color", new Color(1f, 0.82f, 0.45f));
        parent.AddChild(label);
    }

    private static void AddLine(Container parent, string text)
    {
        var label = new Label { Text = text, AutowrapMode = TextServer.AutowrapMode.WordSmart, MouseFilter = Control.MouseFilterEnum.Ignore };
        label.AddThemeFontSizeOverride("font_size", 18);
        parent.AddChild(label);
    }

    private static string Text(string key) => new LocString("main_menu_ui", $"SPIREDUO-{key}").GetFormattedText();
}
