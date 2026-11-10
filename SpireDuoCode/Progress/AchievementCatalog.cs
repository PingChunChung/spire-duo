using SpireDuo.SpireDuoCode.Config;

namespace SpireDuo.SpireDuoCode.Progress;

public enum AchievementPage { Journey, Koala, Panda, BlackCat, Raccoon, Together, Moments }
public enum AchievementTier { Bronze = 1, Silver = 2, Gold = 3, Moon = 4 }

public sealed record AchievementDefinition(string Id, AchievementPage Page, AchievementTier Tier, int Points, bool Coop, bool Hidden, int ProgressTarget = 0, string? StatKey = null, string? PicturePath = null, string? Badge = null);

public static class AchievementCatalog
{
    public const int MaximumPoints = 109;

    private static AchievementDefinition A(string id, AchievementPage page, AchievementTier tier, bool coop = false, bool hidden = false, int target = 0, string? stat = null)
    {
        int points = tier == AchievementTier.Moon ? 2 : (int)tier;
        string badge = $"res://SanaCkoala/images/milestones/badges/{page.ToString().ToLowerInvariant()}_{tier.ToString().ToLowerInvariant()}.png";
        return new AchievementDefinition(id, page, tier, points, coop, hidden, target, stat, $"res://SanaCkoala/images/milestones/{id.ToLowerInvariant()}.png", badge);
    }

    public static readonly AchievementDefinition[] All =
    [
        A(MilestoneIds.FirstSteps, AchievementPage.Journey, AchievementTier.Bronze), A(MilestoneIds.EliteWin, AchievementPage.Journey, AchievementTier.Bronze), A(MilestoneIds.FirstWin, AchievementPage.Journey, AchievementTier.Bronze), A(MilestoneIds.Act1Clear, AchievementPage.Journey, AchievementTier.Bronze), A(MilestoneIds.Act2Clear, AchievementPage.Journey, AchievementTier.Silver), A(MilestoneIds.SpireClear, AchievementPage.Journey, AchievementTier.Gold), A(MilestoneIds.Runs10, AchievementPage.Journey, AchievementTier.Silver, target: 10, stat: nameof(SanaCkoalaStats.Runs)), A(MilestoneIds.Wins5, AchievementPage.Journey, AchievementTier.Gold, target: 5, stat: nameof(SanaCkoalaStats.RunWins)),
        A(MilestoneIds.KoalaFirstNap, AchievementPage.Koala, AchievementTier.Bronze), A(MilestoneIds.DeepSleep, AchievementPage.Koala, AchievementTier.Silver), A(MilestoneIds.KoalaWake15, AchievementPage.Koala, AchievementTier.Silver), A(MilestoneIds.KoalaDrowsy100, AchievementPage.Koala, AchievementTier.Silver, target: 100, stat: nameof(SanaCkoalaStats.KoalaDrowsy)), A(MilestoneIds.KoalaBlock30, AchievementPage.Koala, AchievementTier.Silver), A(MilestoneIds.KoalaDream3, AchievementPage.Koala, AchievementTier.Silver), A(MilestoneIds.KoalaClear, AchievementPage.Koala, AchievementTier.Gold), A(MilestoneIds.KoalaNightWatch, AchievementPage.Koala, AchievementTier.Gold),
        A(MilestoneIds.PandaBamboo10, AchievementPage.Panda, AchievementTier.Bronze), A(MilestoneIds.PandaFirstMunch, AchievementPage.Panda, AchievementTier.Bronze), A(MilestoneIds.BigAppetite, AchievementPage.Panda, AchievementTier.Silver), A(MilestoneIds.PandaMunch50, AchievementPage.Panda, AchievementTier.Silver, target: 50, stat: nameof(SanaCkoalaStats.PandaMunches)), A(MilestoneIds.PandaMunchAll12, AchievementPage.Panda, AchievementTier.Silver), A(MilestoneIds.PandaRoll10, AchievementPage.Panda, AchievementTier.Silver), A(MilestoneIds.PandaClear, AchievementPage.Panda, AchievementTier.Gold), A(MilestoneIds.PandaFeast, AchievementPage.Panda, AchievementTier.Gold),
        A(MilestoneIds.BlackCatJoins, AchievementPage.BlackCat, AchievementTier.Silver), A(MilestoneIds.CatFirstRun, AchievementPage.BlackCat, AchievementTier.Bronze), A(MilestoneIds.CatFirstLine, AchievementPage.BlackCat, AchievementTier.Bronze), A(MilestoneIds.ThreeLines, AchievementPage.BlackCat, AchievementTier.Silver), A(MilestoneIds.CatPounce10, AchievementPage.BlackCat, AchievementTier.Silver), A(MilestoneIds.CatCompanion10, AchievementPage.BlackCat, AchievementTier.Silver, coop: true, target: 10, stat: nameof(SanaCkoalaStats.CatCompanions)), A(MilestoneIds.CatThreshold, AchievementPage.BlackCat, AchievementTier.Gold, hidden: true), A(MilestoneIds.CatVictory, AchievementPage.BlackCat, AchievementTier.Gold),
        A(MilestoneIds.RaccoonJoins, AchievementPage.Raccoon, AchievementTier.Silver), A(MilestoneIds.RaccoonFirstRun, AchievementPage.Raccoon, AchievementTier.Bronze), A(MilestoneIds.RaccoonStash3, AchievementPage.Raccoon, AchievementTier.Bronze), A(MilestoneIds.RaccoonSpend8, AchievementPage.Raccoon, AchievementTier.Silver), A(MilestoneIds.RaccoonTypes, AchievementPage.Raccoon, AchievementTier.Silver), A(MilestoneIds.RaccoonValue3, AchievementPage.Raccoon, AchievementTier.Silver), A(MilestoneIds.RaccoonBossHaul, AchievementPage.Raccoon, AchievementTier.Gold), A(MilestoneIds.RaccoonClear, AchievementPage.Raccoon, AchievementTier.Gold),
        A(MilestoneIds.SideBySide, AchievementPage.Together, AchievementTier.Bronze, coop: true), A(MilestoneIds.HoldHands10, AchievementPage.Together, AchievementTier.Bronze, coop: true, target: 10, stat: nameof(SanaCkoalaStats.HoldHandsPlayed)), A(MilestoneIds.HoldHands, AchievementPage.Together, AchievementTier.Silver, coop: true, target: 50, stat: nameof(SanaCkoalaStats.HoldHandsPlayed)), A(MilestoneIds.BackOnFeet, AchievementPage.Together, AchievementTier.Silver, coop: true), A(MilestoneIds.ShareABite10, AchievementPage.Together, AchievementTier.Silver, coop: true, target: 10, stat: "SharedBites"), A(MilestoneIds.PromiseTurns10, AchievementPage.Together, AchievementTier.Silver, coop: true, target: 10, stat: "PromiseTurns"), A(MilestoneIds.CompanionHandoff, AchievementPage.Together, AchievementTier.Gold, coop: true, hidden: true), A(MilestoneIds.TwoPersonClear, AchievementPage.Together, AchievementTier.Gold, coop: true),
        A(MilestoneIds.Picnic, AchievementPage.Moments, AchievementTier.Bronze), A(MilestoneIds.PhotoBooth, AchievementPage.Moments, AchievementTier.Bronze), A(MilestoneIds.MakeUp, AchievementPage.Moments, AchievementTier.Silver, coop: true), A(MilestoneIds.Stargazing, AchievementPage.Moments, AchievementTier.Silver, coop: true), A(MilestoneIds.SummitPicnic, AchievementPage.Moments, AchievementTier.Silver), A(MilestoneIds.Events6, AchievementPage.Moments, AchievementTier.Silver, target: 6, stat: "EventsFinished"), A(MilestoneIds.Relics7, AchievementPage.Moments, AchievementTier.Silver, target: 7, stat: "UniqueRelics"), A(MilestoneIds.AllPages, AchievementPage.Moments, AchievementTier.Gold, hidden: true)
    ];

    public static AchievementDefinition? Find(string id) => All.FirstOrDefault(definition => definition.Id == id);

    public static int GetPoints() => DevSettings.AlbumAll
        ? MaximumPoints
        : All.Where(definition => ProgressStore.Current.Achievements.ContainsKey(definition.Id)).Sum(definition => definition.Points);

    public static string GetTitleKey()
    {
        AchievementDefinition? title = All
            .Where(definition => definition.Tier == AchievementTier.Gold && (DevSettings.AlbumAll || ProgressStore.Current.Achievements.ContainsKey(definition.Id)))
            .OrderByDescending(definition => ProgressStore.Current.Achievements.GetValueOrDefault(definition.Id, string.Empty))
            .ThenBy(definition => definition.Id)
            .FirstOrDefault();
        return title?.Id ?? "ALBUM.title.None";
    }
}
