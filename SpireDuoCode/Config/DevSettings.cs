namespace SpireDuo.SpireDuoCode.Config;

public static class DevSettings
{
    public static readonly bool UnlockAllCharacters = Environment.GetEnvironmentVariable("SANACKOALA_UNLOCK_ALL") == "1";
}
