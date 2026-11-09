using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using SpireDuo.SpireDuoCode.Cards;
using SpireDuo.SpireDuoCode.Extensions;
using SpireDuo.SpireDuoCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace SpireDuo.SpireDuoCode.Character;

public sealed class BlackCat : PlaceholderCharacterModel
{
    public const string CharacterId = "BlackCat";
    public static readonly Color Color = new("ddd6e8");

    public override bool HideFromVanillaCharacterSelect => true;
    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 68;
    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeCat>(), ModelDb.Card<StrikeCat>(), ModelDb.Card<StrikeCat>(), ModelDb.Card<StrikeCat>(),
        ModelDb.Card<DefendCat>(), ModelDb.Card<DefendCat>(), ModelDb.Card<DefendCat>(), ModelDb.Card<DefendCat>(),
        ModelDb.Card<SoftStep>(), ModelDb.Card<HoldHands>()
    ];
    public override IReadOnlyList<RelicModel> StartingRelics => [ModelDb.Relic<MoonbellCollar>()];
    public override CardPoolModel CardPool => ModelDb.CardPool<BlackCatCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<BlackCatRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<BlackCatPotionPool>();

    public override NCreatureVisuals? CreateCustomVisuals() => NodeFactory<NCreatureVisuals>.CreateFromResource("cat_body.png".CharacterUiOrKoalaPath("koala_body.png"));
    public override Control CustomIcon => NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
    public override string CustomIconTexturePath => "character_icon_cat.png".CharacterUiOrKoalaPath("character_icon_koala.png");
    public override string CustomIconOutlineTexturePath => "character_icon_cat_outline.png".CharacterUiOrKoalaPath("character_icon_koala_outline.png");
    public override string CustomCharacterSelectIconPath => "char_select_cat.png".CharacterUiOrKoalaPath("char_select_koala.png");
    public override string CustomCharacterSelectLockedIconPath => "char_select_cat_locked.png".CharacterUiOrKoalaPath("char_select_koala_locked.png");
    public override string? CustomMapMarkerPath => "map_marker_cat.png".CharacterUiOrKoalaPath("map_marker_koala.png");
    public override CustomEnergyCounter? CustomEnergyCounter => new(i => $"orb/cat_orb_layer_{i}.png".CharacterUiOrKoalaPath($"orb/koala_orb_layer_{i}.png"), new Color("24152f"), new Color("e9c66b"));
    public override string CustomEnergyCounterPath => null!;
    public override string CustomRestSiteAnimPath => "cat_rest_site.tscn".SceneOrKoalaPath("koala_rest_site.tscn");
    public override string CustomMerchantAnimPath => "cat_merchant.tscn".SceneOrKoalaPath("koala_merchant.tscn");
    public override string CustomArmPointingTexturePath => "hands/cat_hand_point.png".CharacterUiOrKoalaPath("hands/koala_hand_point.png");
    public override string CustomArmRockTexturePath => "hands/cat_hand_rock.png".CharacterUiOrKoalaPath("hands/koala_hand_rock.png");
    public override string CustomArmPaperTexturePath => "hands/cat_hand_paper.png".CharacterUiOrKoalaPath("hands/koala_hand_paper.png");
    public override string CustomArmScissorsTexturePath => "hands/cat_hand_scissors.png".CharacterUiOrKoalaPath("hands/koala_hand_scissors.png");
    public override string CustomCharacterSelectBg => "cat_char_select_bg.tscn".SceneOrKoalaPath("koala_char_select_bg.tscn");
    public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/fade_transition_mat.tres";
    public override string CustomTrailPath => SceneHelper.GetScenePath("vfx/card_trail_silent");
    public override string CharacterSelectSfx => "event:/sfx/ui/enchant_shimmer";
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_map";
    public override Color EnergyLabelOutlineColor => new("352246");
    public override Color DialogueColor => new("2b1a3a");
    public override VfxColor SpeechBubbleColor => VfxColor.Purple;
    public override Color MapDrawingColor => new("704a87");
    public override Color RemoteTargetingLineColor => new("c49add");
    public override Color RemoteTargetingLineOutline => new("352246");
}
