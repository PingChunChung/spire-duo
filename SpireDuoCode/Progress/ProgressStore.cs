using System.Text.Json;
using Godot;

namespace SpireDuo.SpireDuoCode.Progress;

public static class ProgressStore
{
    private const string ProgressPath = "user://SanaCkoala/progress.json";
    private const string TemporaryPath = "user://SanaCkoala/progress.json.tmp";
    private const string BadPath = "user://SanaCkoala/progress.json.bad";
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    private static SanaCkoalaProgress? _current;
    private static bool _needsBadFileBackup;

    public static SanaCkoalaProgress Current => _current ??= Load();

    public static void UnlockBlackCat()
    {
        var progress = Current;
        progress.BlackCatUnlocked = true;
        Save();
    }

    /// <summary>Records which of the Koala and the Panda have won Act 3 and returns the updated progress.</summary>
    public static SanaCkoalaProgress RecordSpireClear(bool koala, bool panda)
    {
        var progress = Current;
        if ((!koala || progress.KoalaSpireCleared) && (!panda || progress.PandaSpireCleared))
            return progress;

        progress.KoalaSpireCleared |= koala;
        progress.PandaSpireCleared |= panda;
        Save();
        return progress;
    }

    public static bool UnlockMilestone(string id)
    {
        var progress = Current;
        if (progress.Achievements.ContainsKey(id))
            return false;

        progress.Achievements[id] = DateTime.Now.ToString("yyyy-MM-dd");
        Save();
        return true;
    }

    public static void IncrementStat(Func<SanaCkoalaStats, int> read, Action<SanaCkoalaStats, int> write)
    {
        var progress = Current;
        write(progress.Stats, read(progress.Stats) + 1);
    }

    public static void ResetMilestones()
    {
        var progress = Current;
        progress.Achievements.Clear();
        progress.SeenUnlocks.Clear();
        progress.CollectedRelicIds.Clear();
        progress.CollectedEventIds.Clear();
        progress.Stats = new SanaCkoalaStats();
        Save();
    }

    public static void Flush() => Save();

    private static SanaCkoalaProgress Load()
    {
        if (!Godot.FileAccess.FileExists(ProgressPath)) return new SanaCkoalaProgress { SchemaVersion = 3 };

        try
        {
            using var file = Godot.FileAccess.Open(ProgressPath, Godot.FileAccess.ModeFlags.Read);
            var progress = JsonSerializer.Deserialize<SanaCkoalaProgress>(file.GetAsText(), JsonOptions) ?? new SanaCkoalaProgress();
            progress.Achievements ??= [];
            progress.SeenUnlocks ??= [];
            progress.CollectedRelicIds ??= [];
            progress.CollectedEventIds ??= [];
            progress.Stats ??= new SanaCkoalaStats();
            Migrate(progress);
            return progress;
        }
        catch (Exception exception)
        {
            MainFile.Logger.Warn($"Unable to read progress; preserving the unreadable file and starting fresh: {exception.Message}");
            _needsBadFileBackup = true;
            return new SanaCkoalaProgress { SchemaVersion = 3 };
        }
    }

    public static void MarkSeen(IEnumerable<string> ids)
    {
        bool changed = false;
        foreach (string id in ids.Where(id => Current.Achievements.ContainsKey(id)))
            changed |= Current.SeenUnlocks.Add(id);
        if (changed)
            Save();
    }

    private static void Migrate(SanaCkoalaProgress progress)
    {
        Dictionary<string, string>? legacyMilestones = progress.LegacyMilestones;
        bool migratedLegacyMilestones = legacyMilestones?.Keys.Any(id => id != MilestoneIds.SeeYouNextTime) == true;
        if (legacyMilestones != null)
        {
            foreach ((string id, string date) in legacyMilestones)
            {
                if (id != MilestoneIds.SeeYouNextTime && AchievementCatalog.Find(id) != null)
                    progress.Achievements.TryAdd(id, date);
            }
            // SEE_YOU_NEXT_TIME is not an achievement, but leave its old date in the file.
            // This makes the v1/v2 migration non-destructive without letting it affect the album.
            progress.LegacyMilestones = legacyMilestones
                .Where(pair => pair.Key == MilestoneIds.SeeYouNextTime)
                .ToDictionary();
            if (progress.LegacyMilestones.Count == 0)
                progress.LegacyMilestones = null;
        }
        bool addedBlackCatAchievement = progress.BlackCatUnlocked && progress.Achievements.TryAdd(MilestoneIds.BlackCatJoins, string.Empty);

        // Existing unlocks predate the reveal animation and must not appear as new.
        if (progress.SchemaVersion < 3 || migratedLegacyMilestones || addedBlackCatAchievement)
            progress.SeenUnlocks.UnionWith(progress.Achievements.Keys);
        progress.SchemaVersion = 3;

        // Counters that existed before the achievements did (runs, wins, Hold Hands) can already be past a target.
        foreach (AchievementDefinition definition in AchievementCatalog.All.Where(definition => definition.ProgressTarget > 0 && definition.StatKey != null))
        {
            if (StatValue(progress, definition.StatKey!) >= definition.ProgressTarget)
                progress.Achievements.TryAdd(definition.Id, DateTime.Now.ToString("yyyy-MM-dd"));
        }
    }

    private static int StatValue(SanaCkoalaProgress progress, string key) => key switch
    {
        nameof(SanaCkoalaStats.Runs) => progress.Stats.Runs,
        nameof(SanaCkoalaStats.RunWins) => progress.Stats.RunWins,
        nameof(SanaCkoalaStats.HoldHandsPlayed) => progress.Stats.HoldHandsPlayed,
        _ => 0
    };

    private static void Save()
    {
        SanaCkoalaProgress progress = Current;
        if (_needsBadFileBackup)
        {
            PreserveBadFile();
            _needsBadFileBackup = false;
        }
        DirAccess.MakeDirRecursiveAbsolute(ProjectSettings.GlobalizePath("user://SanaCkoala"));
        using (var file = Godot.FileAccess.Open(TemporaryPath, Godot.FileAccess.ModeFlags.Write))
        {
            file.StoreString(JsonSerializer.Serialize(progress, JsonOptions));
            file.Flush();
        }

        File.Move(ProjectSettings.GlobalizePath(TemporaryPath), ProjectSettings.GlobalizePath(ProgressPath), true);
    }

    private static void PreserveBadFile()
    {
        try
        {
            string source = ProjectSettings.GlobalizePath(ProgressPath);
            string destination = ProjectSettings.GlobalizePath(BadPath);
            if (File.Exists(source))
                File.Move(source, destination, true);
        }
        catch (Exception exception)
        {
            MainFile.Logger.Warn($"Unable to preserve unreadable progress: {exception.Message}");
        }
    }
}
