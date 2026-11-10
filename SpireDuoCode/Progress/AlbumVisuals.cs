using Godot;

namespace SpireDuo.SpireDuoCode.Progress;

public static class AlbumVisuals
{
    private const string Root = "res://SanaCkoala/images/milestones/";

    public static int StyleThreshold => AchievementCatalog.GetPoints() switch
    {
        >= AchievementCatalog.MaximumPoints => 94,
        >= 70 => 70,
        >= 45 => 45,
        >= 20 => 20,
        _ => 0
    };

    public static string? CoverPath => StyleThreshold == 0 ? null : $"{Root}album/covers/cover_{StyleThreshold}.png";

    public static string? MenuDecorationPath => StyleThreshold == 0 ? null : $"{Root}menu_decorations/menu_{StyleThreshold}.png";

    public static TextureRect? CreateOptionalTexture(string path, Vector2 minimumSize)
    {
        if (!ResourceLoader.Exists(path))
            return null;
        return new TextureRect
        {
            Texture = ResourceLoader.Load<Texture2D>(path),
            CustomMinimumSize = minimumSize,
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
            MouseFilter = Control.MouseFilterEnum.Ignore
        };
    }
}
