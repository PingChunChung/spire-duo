using BaseLib.Abstracts;
using SpireDuo.SpireDuoCode.Config;
using SpireDuo.SpireDuoCode.Extensions;
using SpireDuo.SpireDuoCode.Progress;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace SpireDuo.SpireDuoCode.Character;

public sealed class BlackCatCharacterSelectEntry : CustomCharacterSelectEntry
{
    // BaseLib draws the entry button from this path only, so swap in the dimmed portrait while the cat is locked.
    public override string ButtonIconPath => UnlockedInCharacterSelect
        ? "char_select_cat.png".CharacterUiOrKoalaPath("char_select_koala.png")
        : "char_select_cat_locked.png".CharacterUiOrKoalaPath("char_select_koala_locked.png");
    public override string EntryTitle => Text("title");
    public override string EntryDescription => Text("description");
    public override string LockedDescription => Text("lockedDescription");
    public override CharacterModel InitialCharacter => ModelDb.Character<BlackCat>();
    public override CharacterModel AvailabilitySourceCharacter => ModelDb.Character<BlackCat>();
    public override bool UnlockedInCharacterSelect => ProgressStore.Current.BlackCatUnlocked || DevSettings.UnlockAllCharacters;
    private static string Text(string key) => new LocString("characters", $"SPIREDUO-BLACK_CAT.{key}").GetFormattedText();

    public override string CharacterSelectScenePath => "cat_char_select_bg.tscn".SceneOrKoalaPath("koala_char_select_bg.tscn");
}
