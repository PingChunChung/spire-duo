using Godot;
using MegaCrit.Sts2.Core.Localization;

namespace SpireDuo.SpireDuoCode.Progress;

public static class MilestoneBanner
{
    private static readonly Queue<(string? Id, string? Message)> Pending = new();
    private static bool _showing;

    public static void Enqueue(string id)
    {
        Pending.Enqueue((id, null));
        ShowNext();
    }

    public static void EnqueueMessage(string message)
    {
        Pending.Enqueue((null, message));
        ShowNext();
    }

    private static void ShowNext()
    {
        if (_showing || Pending.Count == 0 || Engine.GetMainLoop() is not SceneTree tree)
            return;

        _showing = true;
        var item = Pending.Dequeue();
        var layer = new CanvasLayer { Layer = 100 };
        var row = new HBoxContainer
        {
            AnchorLeft = 0f,
            AnchorRight = 1f,
            OffsetTop = 104f,
            Alignment = BoxContainer.AlignmentMode.Center,
            MouseFilter = Control.MouseFilterEnum.Ignore
        };
        var label = new Label
        {
            Text = item.Message ?? new LocString("main_menu_ui", $"SPIREDUO-{item.Id}.banner").GetFormattedText(),
            HorizontalAlignment = HorizontalAlignment.Center,
            MouseFilter = Control.MouseFilterEnum.Ignore
        };
        label.AddThemeFontSizeOverride("font_size", 36);
        label.AddThemeColorOverride("font_color", new Color(1f, 0.86f, 0.45f));
        label.AddThemeColorOverride("font_outline_color", Colors.Black);
        label.AddThemeConstantOverride("outline_size", 8);
        // The medal reads better than a 3:2 photo at banner size.
        if (item.Id is { } id && AchievementCatalog.Find(id) is { Badge: { } path } && ResourceLoader.Exists(path))
        {
            row.AddChild(new TextureRect
            {
                Texture = ResourceLoader.Load<Texture2D>(path),
                CustomMinimumSize = new Vector2(56f, 56f),
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                MouseFilter = Control.MouseFilterEnum.Ignore
            });
        }
        row.AddChild(label);
        layer.AddChild(row);
        tree.Root.CallDeferred(Node.MethodName.AddChild, layer);
        tree.CreateTimer(4.0).Timeout += () =>
        {
            layer.QueueFree();
            _showing = false;
            ShowNext();
        };
    }
}
