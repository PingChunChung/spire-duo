using BaseLib.Abstracts;
using SpireDuo.SpireDuoCode.Config;
using SpireDuo.SpireDuoCode.Extensions;
using SpireDuo.SpireDuoCode.Progress;
using MegaCrit.Sts2.Core.Models;

namespace SpireDuo.SpireDuoCode.Character;

public sealed class BlackCatCharacterSelectEntry : CustomCharacterSelectEntry
{
    public override string ButtonIconPath => "char_select_cat.png".CharacterUiOrKoalaPath("char_select_koala.png");
    public override string EntryTitle => "The Black Cat";
    public override string EntryDescription => "A quiet observer who turns each deliberate step into a sudden answer.";
    public override string LockedDescription => "Black Cat is locked. Win Act 3 with Koala and Panda together first.";
    public override CharacterModel InitialCharacter => ModelDb.Character<BlackCat>();
    public override bool UnlockedInCharacterSelect => ProgressStore.Current.BlackCatUnlocked || DevSettings.UnlockAllCharacters;
    public override string CharacterSelectScenePath => "cat_char_select_bg.tscn".SceneOrKoalaPath("koala_char_select_bg.tscn");
}
