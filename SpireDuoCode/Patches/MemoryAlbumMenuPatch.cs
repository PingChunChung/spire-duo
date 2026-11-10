using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using SpireDuo.SpireDuoCode.Config;
using SpireDuo.SpireDuoCode.Progress;

namespace SpireDuo.SpireDuoCode.Patches;

[HarmonyPatch(typeof(NMainMenu), nameof(NMainMenu._Ready))]
public static class MemoryAlbumMenuPatch
{
    private const string ButtonName = "SanaCkoalaMemoryAlbumButton";

    [HarmonyPrefix]
    private static void AddAlbumButton(NMainMenu __instance)
    {
        try
        {
            var settings = __instance.GetNodeOrNull<NMainMenuTextButton>("MainMenuTextButtons/SettingsButton");
            if (settings == null || settings.GetParent().GetNodeOrNull(ButtonName) != null)
                return;

            var button = (NMainMenuTextButton)settings.Duplicate(15);
            MemoryAlbumScreen.MenuFont = settings.GetNodeOrNull<Label>("Label")?.GetThemeFont("font") ?? settings.GetThemeFont("font");
            button.Name = ButtonName;
            button.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(_ => MemoryAlbumScreen.Open()));
            Node anchor = settings.GetParent().GetNodeOrNull("ModConfigButton") ?? settings;
            anchor.AddSibling(button);
            button.SetLocalization("SPIREDUO-ALBUM.menu");
            button.CustomMinimumSize = new Vector2(300f, button.CustomMinimumSize.Y);
            button.FocusNeighborLeft = new NodePath(".");
            button.FocusNeighborRight = new NodePath(".");
            AddUnreadDot(button);
            AddMenuPin(button);
        }
        catch (Exception exception)
        {
            MainFile.Logger.Warn($"Unable to add the Memory Album button to the main menu: {exception.Message}");
        }
    }

    // The point rewards show as a small pin beside the album button; nothing appears until the first threshold.
    private static void AddMenuPin(Control button)
    {
        if (AlbumVisuals.MenuDecorationPath is not { } path)
            return;
        TextureRect? pin = AlbumVisuals.CreateOptionalTexture(path, new Vector2(44, 44));
        if (pin == null)
            return;
        pin.AnchorLeft = 0f;
        pin.AnchorRight = 0f;
        pin.OffsetLeft = -36f;
        pin.AnchorTop = 0.5f;
        pin.AnchorBottom = 0.5f;
        pin.OffsetTop = -22f;
        pin.OffsetBottom = 22f;
        pin.OffsetRight = 8f;
        button.AddChild(pin);
    }

    private static void AddUnreadDot(Control button)
    {
        bool unread = ProgressStore.Current.Achievements.Keys.Any(id => !ProgressStore.Current.SeenUnlocks.Contains(id));
        if (!unread)
            return;
        var dot = new ColorRect
        {
            Color = new Color(0.92f, 0.12f, 0.12f),
            CustomMinimumSize = new Vector2(12, 12),
            MouseFilter = Control.MouseFilterEnum.Ignore,
            AnchorLeft = 0.87f,
            AnchorTop = 0.22f,
            AnchorRight = 0.87f,
            AnchorBottom = 0.22f
        };
        button.AddChild(dot);
    }
}

public sealed partial class MemoryAlbumScreen : CanvasLayer
{
    private const string CardBackPath = "res://SanaCkoala/images/milestones/card_back.png";
    private AchievementPage _page = AchievementPage.Journey;
    private AlbumFilter _filter = AlbumFilter.All;
    private GridContainer? _grid;
    private readonly List<(string Id, Control Card)> _unseenCards = [];
    private bool _revealing;
    private int _revealGeneration;
    public static Font? MenuFont { get; set; }

    private enum AlbumFilter { All, Solo, Coop, Unfinished, Finished }

    public static void Open()
    {
        if (Engine.GetMainLoop() is not SceneTree tree || tree.Root.GetNodeOrNull<MemoryAlbumScreen>("SanaCkoalaMemoryAlbumScreen") != null)
            return;
        tree.Root.AddChild(new MemoryAlbumScreen { Name = "SanaCkoalaMemoryAlbumScreen", Layer = 90 });
    }

    public MemoryAlbumScreen()
    {
        var background = new ColorRect { Color = new Color(0.035f, 0.045f, 0.07f, 1f), AnchorRight = 1f, AnchorBottom = 1f, MouseFilter = Control.MouseFilterEnum.Stop };
        if (MenuFont != null)
        {
            var theme = new Theme();
            theme.SetDefaultFont(MenuFont);
            background.Theme = theme;
        }
        AddChild(background);
        AddCover(background);
        var content = new VBoxContainer { AnchorLeft = 0.04f, AnchorTop = 0.035f, AnchorRight = 0.96f, AnchorBottom = 0.955f };
        content.AddThemeConstantOverride("separation", 10);
        background.AddChild(content);

        var title = new Label { Text = Text("ALBUM.title"), HorizontalAlignment = HorizontalAlignment.Center };
        title.AddThemeFontSizeOverride("font_size", 38);
        content.AddChild(title);
        var stats = new Label { Text = HeaderText(), HorizontalAlignment = HorizontalAlignment.Center };
        stats.AddThemeFontSizeOverride("font_size", 20);
        content.AddChild(stats);

        var pages = new HBoxContainer { Alignment = BoxContainer.AlignmentMode.Center };
        pages.AddThemeConstantOverride("separation", 8);
        content.AddChild(pages);
        var pageGroup = new ButtonGroup();
        foreach (AchievementPage page in Enum.GetValues<AchievementPage>())
        {
            int unlocked = AchievementCatalog.All.Count(definition => definition.Page == page && IsUnlocked(definition));
            int total = AchievementCatalog.All.Count(definition => definition.Page == page);
            var button = new Button { Text = $"{Text($"ALBUM.page.{page}")} {unlocked}/{total}", ToggleMode = true, ButtonGroup = pageGroup, ButtonPressed = page == _page };
            button.Pressed += () => { _page = page; RebuildGrid(); };
            pages.AddChild(button);
        }

        var filters = new HBoxContainer { Alignment = BoxContainer.AlignmentMode.Center };
        filters.AddThemeConstantOverride("separation", 8);
        content.AddChild(filters);
        foreach (AlbumFilter filter in Enum.GetValues<AlbumFilter>())
        {
            var button = new Button { Text = Text($"ALBUM.filter.{filter}"), ToggleMode = true, ButtonPressed = filter == _filter };
            button.Pressed += () => { _filter = filter; RebuildGrid(); };
            filters.AddChild(button);
        }

        var scroll = new ScrollContainer { SizeFlagsVertical = Control.SizeFlags.ExpandFill, HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled };
        content.AddChild(scroll);
        var center = new CenterContainer { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };
        scroll.AddChild(center);
        _grid = new GridContainer { Columns = 4 };
        _grid.AddThemeConstantOverride("h_separation", 12);
        _grid.AddThemeConstantOverride("v_separation", 12);
        center.AddChild(_grid);
        RebuildGrid();

        var back = new Button { Text = Text("ALBUM.back"), CustomMinimumSize = new Vector2(180, 42), SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter };
        back.Pressed += CloseOrSkip;
        content.AddChild(back);
        CallDeferred(MethodName.PlayUnseenReveals);
    }

    private static void AddCover(Control background)
    {
        if (AlbumVisuals.CoverPath is not { } path)
            return;
        TextureRect? cover = AlbumVisuals.CreateOptionalTexture(path, new Vector2(1920, 1080));
        if (cover == null)
            return;
        cover.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        cover.Modulate = new Color(1f, 1f, 1f, 0.3f);
        background.AddChild(cover);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            CloseOrSkip();
            GetViewport().SetInputAsHandled();
        }
    }

    private void CloseOrSkip()
    {
        if (_revealing)
        {
            _revealing = false;
            foreach ((string _, Control card) in _unseenCards)
            {
                card.Modulate = Colors.White;
                card.Scale = Vector2.One;
            }
            ProgressStore.MarkSeen(_unseenCards.Select(item => item.Id));
            return;
        }
        QueueFree();
    }

    private void RebuildGrid()
    {
        if (_grid == null)
            return;
        foreach (Node child in _grid.GetChildren())
            child.QueueFree();
        // Switching page or filter mid-reveal: the cards being revealed are gone, so count them as seen.
        if (_revealing)
        {
            _revealing = false;
            ProgressStore.MarkSeen(_unseenCards.Select(item => item.Id));
        }
        _unseenCards.Clear();
        if (_page == AchievementPage.BlackCat && !ProgressStore.Current.BlackCatUnlocked && !DevSettings.AlbumAll)
        {
            _grid.AddChild(CreateCatTeaser());
            return;
        }

        foreach (AchievementDefinition definition in AchievementCatalog.All.Where(definition => definition.Page == _page && MatchesFilter(definition)))
            _grid.AddChild(CreateEntry(definition));
        if (IsInsideTree())
            PlayUnseenReveals();
    }

    private bool MatchesFilter(AchievementDefinition definition) => _filter switch
    {
        AlbumFilter.Solo => !definition.Coop,
        AlbumFilter.Coop => definition.Coop,
        AlbumFilter.Unfinished => !IsUnlocked(definition),
        AlbumFilter.Finished => IsUnlocked(definition),
        _ => true
    };

    private Control CreateCatTeaser()
    {
        var teaser = CardFrame(new Color(0.07f, 0.1f, 0.14f), new Color(0.42f, 0.52f, 0.67f));
        teaser.CustomMinimumSize = new Vector2(300, 300);
        var panel = new VBoxContainer();
        teaser.AddChild(panel);
        AddPicture(panel, "res://SanaCkoala/images/milestones/cat_teaser.png");
        panel.AddChild(new Label { Text = Text("ALBUM.catTeaser"), HorizontalAlignment = HorizontalAlignment.Center, AutowrapMode = TextServer.AutowrapMode.WordSmart });
        panel.AddChild(new Label { Text = $"{Text("ALBUM.koalaCleared")} {Check(ProgressStore.Current.KoalaSpireCleared)}\n{Text("ALBUM.pandaCleared")} {Check(ProgressStore.Current.PandaSpireCleared)}", HorizontalAlignment = HorizontalAlignment.Center });
        return teaser;
    }

    private Control CreateEntry(AchievementDefinition definition)
    {
        bool unlocked = IsUnlocked(definition);
        ProgressStore.Current.Achievements.TryGetValue(definition.Id, out string? date);
        var frame = CardFrame(unlocked ? new Color(0.12f, 0.1f, 0.16f) : new Color(0.07f, 0.07f, 0.09f), unlocked ? TierColor(definition.Tier) : new Color(0.25f, 0.25f, 0.3f));
        frame.CustomMinimumSize = new Vector2(300, 0);
        var panel = new VBoxContainer();
        panel.AddThemeConstantOverride("separation", 5);
        frame.AddChild(panel);
        bool hasPhoto = unlocked && definition.PicturePath != null && ResourceLoader.Exists(definition.PicturePath);
        if (hasPhoto)
        {
            AddPicture(panel, definition.PicturePath);
            if (definition.Badge is { } badge)
                AddBadge(panel, badge, 48);
        }
        else if (unlocked)
            AddPicture(panel, definition.Badge, 1f, true);
        else
            AddPicture(panel, CardBackPath, definition.Hidden ? 0.35f : 0.8f);
        string title = !unlocked && definition.Hidden ? "？？？" : Text($"{definition.Id}.title");
        var titleLabel = new Label { Text = title, HorizontalAlignment = HorizontalAlignment.Center, AutowrapMode = TextServer.AutowrapMode.WordSmart };
        titleLabel.AddThemeFontSizeOverride("font_size", 21);
        panel.AddChild(titleLabel);
        panel.AddChild(new Label { Text = Text($"ALBUM.tier.{definition.Tier}"), HorizontalAlignment = HorizontalAlignment.Center });
        panel.AddChild(new Label { Text = unlocked ? Text($"{definition.Id}.description") : definition.Hidden ? Text("ALBUM.hiddenHint") : Text($"{definition.Id}.hint"), HorizontalAlignment = HorizontalAlignment.Center, AutowrapMode = TextServer.AutowrapMode.WordSmart });
        if (!unlocked && definition.ProgressTarget > 0 && definition.StatKey != null)
        {
            int value = Math.Min(definition.ProgressTarget, MilestoneTracker.GetStat(definition.StatKey));
            panel.AddChild(new ProgressBar { MinValue = 0, MaxValue = definition.ProgressTarget, Value = value, ShowPercentage = false });
            panel.AddChild(new Label { Text = $"{value} / {definition.ProgressTarget}" + (value * 10 >= definition.ProgressTarget * 7 ? $" · {Text("ALBUM.almost")}" : string.Empty), HorizontalAlignment = HorizontalAlignment.Center });
        }
        if (unlocked)
            panel.AddChild(new Label { Text = string.IsNullOrEmpty(date) ? Text("ALBUM.dateUnknown") : date, HorizontalAlignment = HorizontalAlignment.Center });
        if (unlocked && !ProgressStore.Current.SeenUnlocks.Contains(definition.Id) && !DevSettings.AlbumAll)
        {
            frame.Modulate = new Color(1, 1, 1, 0);
            _unseenCards.Add((definition.Id, frame));
        }
        return frame;
    }

    private static void AddBadge(Container panel, string path, float size)
    {
        TextureRect? badge = AlbumVisuals.CreateOptionalTexture(path, new Vector2(size, size));
        if (badge == null)
            return;
        badge.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
        panel.AddChild(badge);
    }

    private async void PlayUnseenReveals()
    {
        if (_unseenCards.Count == 0 || DevSettings.AlbumAll)
            return;
        int generation = ++_revealGeneration;
        _revealing = true;
        // Wait one frame so the new cards have their final size before scaling around their centre.
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        foreach ((string id, Control card) in _unseenCards.ToList())
        {
            if (!_revealing || generation != _revealGeneration)
                return;
            card.PivotOffset = card.Size / 2;
            card.Scale = new Vector2(0.85f, 0.85f);
            Tween tween = CreateTween().SetParallel();
            tween.TweenProperty(card, "modulate:a", 1f, 0.5f);
            tween.TweenProperty(card, "scale", Vector2.One, 0.8f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
            await ToSignal(tween, Tween.SignalName.Finished);
            if (!_revealing || generation != _revealGeneration)
                return;
            ProgressStore.MarkSeen([id]);
            await ToSignal(GetTree().CreateTimer(0.25), SceneTreeTimer.SignalName.Timeout);
        }
        if (generation == _revealGeneration)
            _revealing = false;
    }

    private static bool IsUnlocked(AchievementDefinition definition) => DevSettings.AlbumAll || ProgressStore.Current.Achievements.ContainsKey(definition.Id);
    private static string Check(bool value) => value ? "✓" : "✗";
    private static PanelContainer CardFrame(Color background, Color border)
    {
        var frame = new PanelContainer();
        frame.AddThemeStyleboxOverride("panel", new StyleBoxFlat { BgColor = background, BorderColor = border, BorderWidthLeft = 2, BorderWidthTop = 2, BorderWidthRight = 2, BorderWidthBottom = 2, CornerRadiusTopLeft = 8, CornerRadiusTopRight = 8, CornerRadiusBottomLeft = 8, CornerRadiusBottomRight = 8, ContentMarginLeft = 8, ContentMarginTop = 8, ContentMarginRight = 8, ContentMarginBottom = 8 });
        return frame;
    }

    // Photos fill the 3:2 slot; badges (achievements without a photo) sit centred at a smaller size.
    private static void AddPicture(Container panel, string? path, float brightness = 1f, bool isBadge = false)
    {
        var picture = new PanelContainer { CustomMinimumSize = new Vector2(280, 185) };
        picture.AddThemeStyleboxOverride("panel", new StyleBoxFlat { BgColor = new Color(0.04f, 0.04f, 0.06f) });
        if (path != null && ResourceLoader.Exists(path))
        {
            var texture = new TextureRect
            {
                Texture = ResourceLoader.Load<Texture2D>(path),
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                Modulate = new Color(brightness, brightness, brightness)
            };
            if (isBadge)
            {
                texture.CustomMinimumSize = new Vector2(150, 150);
                texture.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
                texture.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
            }
            picture.AddChild(texture);
        }
        panel.AddChild(picture);
    }

    private static Color TierColor(AchievementTier tier) => tier switch
    {
        AchievementTier.Gold => new Color(0.93f, 0.76f, 0.36f),
        AchievementTier.Silver => new Color(0.78f, 0.81f, 0.88f),
        _ => new Color(0.74f, 0.5f, 0.32f)
    };

    private static string HeaderText()
    {
        int unlocked = DevSettings.AlbumAll ? AchievementCatalog.All.Length : ProgressStore.Current.Achievements.Keys.Count(id => AchievementCatalog.Find(id) != null);
        int points = AchievementCatalog.GetPoints();
        string titleKey = AchievementCatalog.GetTitleKey();
        string title = titleKey == "ALBUM.title.None" ? Text(titleKey) : Text($"{titleKey}.title");
        return string.Format(RawText("ALBUM.header"), unlocked, points, title);
    }

    private static string Text(string key) => new LocString("main_menu_ui", $"SPIREDUO-{key}").GetFormattedText();
    private static string RawText(string key) => new LocString("main_menu_ui", $"SPIREDUO-{key}").GetRawText();
}
