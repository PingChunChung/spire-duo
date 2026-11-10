using BaseLib.Abstracts;
using SpireDuo.SpireDuoCode.Config;
using SpireDuo.SpireDuoCode.Extensions;
using SpireDuo.SpireDuoCode.Progress;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace SpireDuo.SpireDuoCode.Character;

public sealed class RaccoonCharacterSelectEntry : CustomCharacterSelectEntry
{
    public override string ButtonIconPath => UnlockedInCharacterSelect? "char_select_raccoon.png".CharacterUiOrKoalaPath("char_select_koala.png") : "char_select_raccoon_locked.png".CharacterUiOrKoalaPath("char_select_koala_locked.png");
    public override string EntryTitle => Text("title");
    public override string EntryDescription => Text("description");
    public override string LockedDescription => Text("lockedDescription");
    public override CharacterModel InitialCharacter => ModelDb.Character<Raccoon>();
    public override CharacterModel AvailabilitySourceCharacter => ModelDb.Character<Raccoon>();
    public override bool UnlockedInCharacterSelect => ProgressStore.Current.RaccoonUnlocked || DevSettings.UnlockAllCharacters;
    // Only one locked silhouette at a time: the Raccoon's hint names the Black Cat, so it appears once the cat is unlocked.
    public override bool VisibleInCharacterSelect => UnlockedInCharacterSelect || ProgressStore.Current.BlackCatUnlocked;
    public override string CharacterSelectScenePath => "raccoon_char_select_bg.tscn".SceneOrKoalaPath("koala_char_select_bg.tscn");
    private static string Text(string key) => new LocString("characters", $"SPIREDUO-RACCOON.{key}").GetFormattedText();
}
