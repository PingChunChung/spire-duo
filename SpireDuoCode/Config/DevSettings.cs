using BaseLib.Config;

namespace SpireDuo.SpireDuoCode.Config;

public sealed class DevSettings : SimpleModConfig
{
    [ConfigSection("Developer")]
    public static bool UnlockAllCharacters { get; set; } = false;
}
