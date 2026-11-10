namespace SpireDuo.SpireDuoCode.Progress;

// Kept as the public name for existing cards and patches. These ids are now achievements.
public static class MilestoneIds
{
    public const string FirstSteps = "FIRST_STEPS";
    public const string EliteWin = "ELITE_WIN";
    public const string FirstWin = "FIRST_WIN";
    public const string Act1Clear = "ACT1_CLEAR";
    public const string Act2Clear = "ACT2_CLEAR";
    public const string SpireClear = "SPIRE_CLEAR";
    public const string Runs10 = "RUNS_10";
    public const string Wins5 = "WINS_5";
    public const string KoalaFirstNap = "KOALA_FIRST_NAP";
    public const string DeepSleep = "DEEP_SLEEP";
    public const string KoalaWake15 = "KOALA_WAKE_15";
    public const string KoalaDrowsy100 = "KOALA_DROWSY_100";
    public const string KoalaBlock30 = "KOALA_BLOCK_30";
    public const string KoalaDream3 = "KOALA_DREAM_3";
    public const string KoalaClear = "KOALA_CLEAR";
    public const string KoalaNightWatch = "KOALA_NIGHT_WATCH";
    public const string PandaBamboo10 = "PANDA_BAMBOO_10";
    public const string PandaFirstMunch = "PANDA_FIRST_MUNCH";
    public const string BigAppetite = "BIG_APPETITE";
    public const string PandaMunch50 = "PANDA_MUNCH_50";
    public const string PandaMunchAll12 = "PANDA_MUNCH_ALL_12";
    public const string PandaRoll10 = "PANDA_ROLL_10";
    public const string PandaClear = "PANDA_CLEAR";
    public const string PandaFeast = "PANDA_FEAST";
    public const string BlackCatJoins = "BLACK_CAT_JOINS";
    public const string CatFirstRun = "CAT_FIRST_RUN";
    public const string CatFirstLine = "CAT_FIRST_LINE";
    public const string ThreeLines = "THREE_LINES";
    public const string CatPounce10 = "CAT_POUNCE_10";
    public const string CatCompanion10 = "CAT_COMPANION_10";
    public const string CatThreshold = "CAT_THRESHOLD";
    public const string CatVictory = "CAT_VICTORY";
    public const string SideBySide = "SIDE_BY_SIDE";
    public const string HoldHands10 = "HOLD_HANDS_10";
    public const string HoldHands = "HOLD_HANDS";
    public const string BackOnFeet = "BACK_ON_FEET";
    public const string ShareABite10 = "SHARE_A_BITE_10";
    public const string PromiseTurns10 = "PROMISE_TURNS_10";
    public const string CompanionHandoff = "COMPANION_HANDOFF";
    public const string TwoPersonClear = "TWO_PERSON_CLEAR";
    public const string Picnic = "PICNIC";
    public const string PhotoBooth = "PHOTO_BOOTH";
    public const string MakeUp = "MAKE_UP";
    public const string Stargazing = "STARGAZING";
    public const string SummitPicnic = "SUMMIT_PICNIC";
    public const string Events6 = "EVENTS_6";
    public const string Relics7 = "RELICS_7";
    public const string AllPages = "ALL_PAGES";
    public const string SeeYouNextTime = "SEE_YOU_NEXT_TIME";

    public static readonly string[] All = AchievementCatalog.All.Select(definition => definition.Id).ToArray();
}
