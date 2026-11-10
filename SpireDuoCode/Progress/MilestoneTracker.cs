using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Progress;

public static class MilestoneTracker
{
    private static readonly Dictionary<ICombatState, Dictionary<ulong, int>> MunchesThisTurn = [];
    private static readonly HashSet<ICombatState> CombatFalls = [];
    private static readonly Dictionary<ICombatState, HashSet<CardModel>> RetainedCards = [];
    private static readonly Dictionary<ICombatState, int> NightWatchTurns = [];
    private static readonly HashSet<ICombatState> ThresholdPreventions = [];
    private static readonly Dictionary<ICombatState, Dictionary<string, int>> CombatCounts = [];
    private static readonly HashSet<string> RunUnlocks = [];
    public static bool LastRunWasLost { get; private set; }

    public static bool IsModRun(IRunState state) => state.Players.Any(player => player.Character is Koala or Panda or BlackCat);

    public static bool IsCoopModRun(IRunState state) => IsModRun(state) && state.Players.Count > 1;

    public static void Unlock(string id)
    {
        if (ProgressStore.UnlockMilestone(id))
        {
            RunUnlocks.Add(id);
            MilestoneBanner.Enqueue(id);
        }
        if (id != MilestoneIds.AllPages && AchievementCatalog.All.Where(definition => definition.Id != MilestoneIds.AllPages).All(definition => ProgressStore.Current.Achievements.ContainsKey(definition.Id)))
            Unlock(MilestoneIds.AllPages);
    }

    public static void BeginRun()
    {
        RunUnlocks.Clear();
        MunchesThisTurn.Clear();
        CombatFalls.Clear();
        RetainedCards.Clear();
        NightWatchTurns.Clear();
        ThresholdPreventions.Clear();
        CombatCounts.Clear();
        LastRunWasLost = false;
    }

    public static void RecordRunEnd(bool lost) => LastRunWasLost = lost;

    public static IReadOnlyList<AchievementDefinition> GetRunUnlocks() => RunUnlocks
        .Select(AchievementCatalog.Find)
        .OfType<AchievementDefinition>()
        .ToList();

    public static void RecordHoldHands()
    {
        ProgressStore.IncrementStat(stats => stats.HoldHandsPlayed, (stats, value) => stats.HoldHandsPlayed = value);
        if (ProgressStore.Current.Stats.HoldHandsPlayed >= 10)
            Unlock(MilestoneIds.HoldHands10);
        if (ProgressStore.Current.Stats.HoldHandsPlayed >= 50)
            Unlock(MilestoneIds.HoldHands);
    }

    public static void RecordSharedBite(Creature creature)
    {
        if (creature.Player?.RunState is not { } state || !IsCoopModRun(state))
            return;
        ProgressStore.IncrementStat(stats => stats.SharedBites, (stats, value) => stats.SharedBites = value);
        if (ProgressStore.Current.Stats.SharedBites >= 10)
            Unlock(MilestoneIds.ShareABite10);
    }

    public static void RecordPromiseTurn(Creature creature)
    {
        if (creature.Player?.RunState is not { } state || !IsCoopModRun(state))
            return;
        ProgressStore.IncrementStat(stats => stats.PromiseTurns, (stats, value) => stats.PromiseTurns = value);
        if (ProgressStore.Current.Stats.PromiseTurns >= 10)
            Unlock(MilestoneIds.PromiseTurns10);
    }

    public static void RecordCompanionHandoff(Creature creature, Creature? ally)
    {
        if (creature.IsDead || ally is not { IsDead: false } || ally.Player == null || ally.Player == creature.Player)
            return;
        if (creature.Player?.RunState is not { } state || !IsCoopModRun(state) || creature.CombatState is not { } combat)
            return;
        if (IncrementCombatCount(combat, "CompanionHandoff") >= 3)
            Unlock(MilestoneIds.CompanionHandoff);
    }

    public static void RecordEventFinished(string id, IRunState state)
    {
        if (!IsModRun(state) || !ProgressStore.Current.CollectedEventIds.Add(id))
            return;
        ProgressStore.Current.Stats.EventsFinished = ProgressStore.Current.CollectedEventIds.Count;
        if (ProgressStore.Current.Stats.EventsFinished >= 6)
            Unlock(MilestoneIds.Events6);
    }

    public static void RecordRelicCollected(string id, IRunState state)
    {
        if (!IsModRun(state) || !ProgressStore.Current.CollectedRelicIds.Add(id))
            return;
        ProgressStore.Current.Stats.UniqueRelics = ProgressStore.Current.CollectedRelicIds.Count;
        if (ProgressStore.Current.Stats.UniqueRelics >= 7)
            Unlock(MilestoneIds.Relics7);
    }

    public static void RecordDrowsy(Creature creature, decimal currentAmount, decimal gainedAmount)
    {
        if (creature.IsDead || creature.Player?.Character is not Koala)
            return;

        if (currentAmount >= 10)
            Unlock(MilestoneIds.KoalaFirstNap);
        if (currentAmount >= 20)
            Unlock(MilestoneIds.DeepSleep);
        ProgressStore.IncrementStat(stats => stats.KoalaDrowsy, (stats, value) => stats.KoalaDrowsy = value - 1 + (int)gainedAmount);
        if (ProgressStore.Current.Stats.KoalaDrowsy >= 100)
            Unlock(MilestoneIds.KoalaDrowsy100);
    }

    public static void RecordWake(Creature creature, int amount)
    {
        if (!creature.IsDead && creature.Player?.Character is Koala && amount >= 15)
            Unlock(MilestoneIds.KoalaWake15);
    }

    public static void RecordDrowsyTurnEnd(Creature creature, decimal amount)
    {
        if (creature.IsDead || creature.Player?.Character is not Koala)
            return;

        if (amount >= 30)
            Unlock(MilestoneIds.KoalaBlock30);
        ICombatState? combat = creature.CombatState;
        if (combat == null || combat.RunState.CurrentRoom?.RoomType != RoomType.Boss)
            return;

        int turns = amount >= 10 && NightWatchTurns.TryGetValue(combat, out int previous) ? previous + 1 : amount >= 10 ? 1 : 0;
        NightWatchTurns[combat] = turns;
        if (turns >= 5)
            Unlock(MilestoneIds.KoalaNightWatch);
    }

    public static void RecordRetainGranted(Creature creature, CardModel card)
    {
        if (creature.IsDead || creature.Player?.Character is not Koala || creature.CombatState is not { } combat)
            return;

        if (!RetainedCards.TryGetValue(combat, out HashSet<CardModel>? cards))
            RetainedCards[combat] = cards = [];
        cards.Add(card);
        if (cards.Count >= 3)
            Unlock(MilestoneIds.KoalaDream3);
    }

    public static void RecordBamboo(Creature creature, decimal currentAmount)
    {
        if (!creature.IsDead && creature.Player?.Character is Panda && currentAmount >= 10)
            Unlock(MilestoneIds.PandaBamboo10);
    }

    public static void RecordMunch(Creature creature, int amount)
    {
        if (creature.IsDead || creature.Player?.Character is not Panda)
            return;

        ProgressStore.IncrementStat(stats => stats.PandaMunches, (stats, value) => stats.PandaMunches = value);
        if (ProgressStore.Current.Stats.PandaMunches >= 50)
            Unlock(MilestoneIds.PandaMunch50);
        Unlock(MilestoneIds.PandaFirstMunch);
        if (amount >= 12)
            Unlock(MilestoneIds.PandaMunchAll12);

        if (creature.CombatState is not { } combat)
            return;

        int turn = creature.Player.PlayerCombatState?.TurnNumber ?? 0;
        ulong key = creature.Player.NetId;
        if (!MunchesThisTurn.TryGetValue(combat, out Dictionary<ulong, int>? counts))
            MunchesThisTurn[combat] = counts = [];
        int packed = counts.TryGetValue(key, out int value) ? value : -1;
        int count = packed >> 16 == turn ? (packed & 0xffff) + 1 : 1;
        counts[key] = (turn << 16) | count;
        if (count >= 3)
            Unlock(MilestoneIds.BigAppetite);

        if (combat.RunState.CurrentRoom?.RoomType == RoomType.Boss)
        {
            int feast = IncrementCombatCount(combat, "PandaFeast");
            if (feast >= 12)
                Unlock(MilestoneIds.PandaFeast);
        }
    }

    public static void RecordRoll(Creature creature, decimal currentAmount)
    {
        if (!creature.IsDead && creature.Player?.Character is Panda && currentAmount >= 10)
            Unlock(MilestoneIds.PandaRoll10);
    }

    public static void RecordFootprintLine(FootprintStatePower state, FootprintLine line)
    {
        if (state.Owner.IsDead || state.Owner.Player?.Character is not BlackCat)
            return;

        Unlock(MilestoneIds.CatFirstLine);
        if (state.HasTriggeredThisCombat(FootprintLine.Pounce) && state.HasTriggeredThisCombat(FootprintLine.Groom) && state.HasTriggeredThisCombat(FootprintLine.Omen))
            Unlock(MilestoneIds.ThreeLines);
        if (line == FootprintLine.Pounce && state.PouncesThisCombat >= 10)
            Unlock(MilestoneIds.CatPounce10);
    }

    public static void RecordCompanionResolved(Creature creature, Creature? ally)
    {
        if (creature.IsDead || ally is not { IsDead: false } || creature.Player?.Character is not BlackCat || creature.Player.RunState is not { } state || !IsCoopModRun(state))
            return;

        ProgressStore.IncrementStat(stats => stats.CatCompanions, (stats, value) => stats.CatCompanions = value);
        if (ProgressStore.Current.Stats.CatCompanions >= 10)
            Unlock(MilestoneIds.CatCompanion10);
    }

    public static void RecordThresholdPrevention(Creature creature)
    {
        if (creature.Player?.Character is BlackCat && creature.CombatState is { } combat)
            ThresholdPreventions.Add(combat);
    }

    public static void RecordPlayerFall(Creature creature)
    {
        if (creature.Player?.RunState is { } state && creature.CombatState != null && IsCoopModRun(state))
            CombatFalls.Add(creature.CombatState);
    }

    public static bool ConsumeCombatFall(ICombatState combatState) => CombatFalls.Remove(combatState);

    public static void CompleteCombat(ICombatState combatState)
    {
        if (ThresholdPreventions.Remove(combatState))
            Unlock(MilestoneIds.CatThreshold);
        CombatFalls.Remove(combatState);
        RetainedCards.Remove(combatState);
        NightWatchTurns.Remove(combatState);
        CombatCounts.Remove(combatState);
        MunchesThisTurn.Remove(combatState);
    }

    private static int IncrementCombatCount(ICombatState combat, string key)
    {
        if (!CombatCounts.TryGetValue(combat, out Dictionary<string, int>? counts))
            CombatCounts[combat] = counts = [];
        int count = counts.GetValueOrDefault(key) + 1;
        counts[key] = count;
        return count;
    }

    public static int GetStat(string key) => key switch
    {
        nameof(SanaCkoalaStats.Runs) => ProgressStore.Current.Stats.Runs,
        nameof(SanaCkoalaStats.RunWins) => ProgressStore.Current.Stats.RunWins,
        nameof(SanaCkoalaStats.HoldHandsPlayed) => ProgressStore.Current.Stats.HoldHandsPlayed,
        nameof(SanaCkoalaStats.KoalaDrowsy) => ProgressStore.Current.Stats.KoalaDrowsy,
        nameof(SanaCkoalaStats.PandaMunches) => ProgressStore.Current.Stats.PandaMunches,
        nameof(SanaCkoalaStats.CatCompanions) => ProgressStore.Current.Stats.CatCompanions,
        nameof(SanaCkoalaStats.SharedBites) => ProgressStore.Current.Stats.SharedBites,
        nameof(SanaCkoalaStats.PromiseTurns) => ProgressStore.Current.Stats.PromiseTurns,
        nameof(SanaCkoalaStats.EventsFinished) => ProgressStore.Current.Stats.EventsFinished,
        nameof(SanaCkoalaStats.UniqueRelics) => ProgressStore.Current.Stats.UniqueRelics,
        _ => 0
    };
}
