using System.Text.Json.Serialization;

namespace SpireDuo.SpireDuoCode.Progress;

public sealed class SanaCkoalaProgress
{
    // A missing schemaVersion is a v1 file. New files are explicitly created at v5 by ProgressStore.
    public int SchemaVersion { get; set; }
    public bool BlackCatUnlocked { get; set; }
    public bool KoalaSpireCleared { get; set; }
    public bool PandaSpireCleared { get; set; }
    public bool RaccoonUnlocked { get; set; }
    public Dictionary<string, string> Achievements { get; set; } = [];
    public HashSet<string> SeenUnlocks { get; set; } = [];
    public HashSet<string> CollectedRelicIds { get; set; } = [];
    public HashSet<string> CollectedEventIds { get; set; } = [];
    public SanaCkoalaStats Stats { get; set; } = new();

    // Retains only the old SEE_YOU_NEXT_TIME record after migration; it is never an album achievement.
    [JsonPropertyName("milestones")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string>? LegacyMilestones { get; set; }
}

public sealed class SanaCkoalaStats
{
    public int Runs { get; set; }
    public int RunWins { get; set; }
    public int Defeats { get; set; }
    public int CombatsWon { get; set; }
    public int EliteWins { get; set; }
    public int CoopRuns { get; set; }
    public int HoldHandsPlayed { get; set; }
    public int KoalaDrowsy { get; set; }
    public int PandaMunches { get; set; }
    public int CatCompanions { get; set; }
    public int SharedBites { get; set; }
    public int PromiseTurns { get; set; }
    public int EventsFinished { get; set; }
    public int UniqueRelics { get; set; }
}
