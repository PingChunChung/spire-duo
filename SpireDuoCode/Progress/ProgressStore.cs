using System.Text.Json;
using Godot;

namespace SpireDuo.SpireDuoCode.Progress;

public static class ProgressStore
{
    private const string ProgressPath = "user://SanaCkoala/progress.json";
    private const string TemporaryPath = "user://SanaCkoala/progress.json.tmp";
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public static SanaCkoalaProgress Current => Load();

    public static void UnlockBlackCat()
    {
        var progress = Load();
        progress.BlackCatUnlocked = true;
        Save(progress);
    }

    /// <summary>Records which of the Koala and the Panda have won Act 3 and returns the updated progress.</summary>
    public static SanaCkoalaProgress RecordSpireClear(bool koala, bool panda)
    {
        var progress = Load();
        if ((!koala || progress.KoalaSpireCleared) && (!panda || progress.PandaSpireCleared))
            return progress;

        progress.KoalaSpireCleared |= koala;
        progress.PandaSpireCleared |= panda;
        Save(progress);
        return progress;
    }

    private static SanaCkoalaProgress Load()
    {
        if (!Godot.FileAccess.FileExists(ProgressPath)) return new SanaCkoalaProgress();

        try
        {
            using var file = Godot.FileAccess.Open(ProgressPath, Godot.FileAccess.ModeFlags.Read);
            return JsonSerializer.Deserialize<SanaCkoalaProgress>(file.GetAsText(), JsonOptions) ?? new SanaCkoalaProgress();
        }
        catch (Exception exception)
        {
            MainFile.Logger.Warn($"Unable to read Black Cat progress; treating it as locked: {exception.Message}");
            return new SanaCkoalaProgress();
        }
    }

    private static void Save(SanaCkoalaProgress progress)
    {
        DirAccess.MakeDirRecursiveAbsolute(ProjectSettings.GlobalizePath("user://SanaCkoala"));
        using (var file = Godot.FileAccess.Open(TemporaryPath, Godot.FileAccess.ModeFlags.Write))
        {
            file.StoreString(JsonSerializer.Serialize(progress, JsonOptions));
            file.Flush();
        }

        File.Move(ProjectSettings.GlobalizePath(TemporaryPath), ProjectSettings.GlobalizePath(ProgressPath), true);
    }
}
