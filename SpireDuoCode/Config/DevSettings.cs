namespace SpireDuo.SpireDuoCode.Config;

public static class DevSettings
{
    public static readonly bool UnlockAllCharacters = Environment.GetEnvironmentVariable("SANACKOALA_UNLOCK_ALL") == "1";
    public static readonly bool AlbumAll = Environment.GetEnvironmentVariable("SANACKOALA_ALBUM_ALL") == "1";
}
