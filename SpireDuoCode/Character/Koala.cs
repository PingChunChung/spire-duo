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

public class Koala : PlaceholderCharacterModel
{
    public const string CharacterId = "Koala";

    public static readonly Color Color = new("8fa3ad");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 70;

    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikeKoala>(),
        ModelDb.Card<StrikeKoala>(),
        ModelDb.Card<StrikeKoala>(),
        ModelDb.Card<StrikeKoala>(),
        ModelDb.Card<DefendKoala>(),
        ModelDb.Card<DefendKoala>(),
        ModelDb.Card<DefendKoala>(),
        ModelDb.Card<DefendKoala>(),
        ModelDb.Card<Doze>(),
        ModelDb.Card<HoldHands>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<EucalyptusLeaf>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<KoalaCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<KoalaRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<KoalaPotionPool>();

    public override NCreatureVisuals? CreateCustomVisuals() =>
        NodeFactory<NCreatureVisuals>.CreateFromResource("koala_body.png".CharacterUiPath());

    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    public override string CustomIconTexturePath => "character_icon_koala.png".CharacterUiPath();
    public override string CustomIconOutlineTexturePath => "character_icon_koala_outline.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_koala.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_koala_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_koala.png".CharacterUiPath();

    // Everything below replaces what PlaceholderCharacterModel would otherwise borrow from the Ironclad.
    public override CustomEnergyCounter? CustomEnergyCounter =>
        new CustomEnergyCounter(i => $"orb/koala_orb_layer_{i}.png".CharacterUiPath(), new Color("2e4a57ff"), new Color("bfe6f0"));
    public override string? CustomEnergyCounterPath => null;
    public override string CustomRestSiteAnimPath => "koala_rest_site.tscn".ScenePath();
    public override string CustomMerchantAnimPath => "koala_merchant.tscn".ScenePath();
    public override string CustomArmPointingTexturePath => "hands/koala_hand_point.png".CharacterUiPath();
    public override string CustomArmRockTexturePath => "hands/koala_hand_rock.png".CharacterUiPath();
    public override string CustomArmPaperTexturePath => "hands/koala_hand_paper.png".CharacterUiPath();
    public override string CustomArmScissorsTexturePath => "hands/koala_hand_scissors.png".CharacterUiPath();
    public override string CustomCharacterSelectBg => "koala_char_select_bg.tscn".ScenePath();
    public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/fade_transition_mat.tres";
    public override string CustomTrailPath => SceneHelper.GetScenePath("vfx/card_trail_defect");
    public override string CharacterSelectSfx => "event:/sfx/ui/enchant_shimmer";
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_map";

    // Co-op markers (map drawings, pings, targeting line, energy label) default to black for every modded character.
    public override Color EnergyLabelOutlineColor => new("2e4a57ff");
    public override Color DialogueColor => new("2b4652");
    public override VfxColor SpeechBubbleColor => VfxColor.Cyan;
    public override Color MapDrawingColor => new("3f7a8c");
    public override Color RemoteTargetingLineColor => new("9fc6d6ff");
    public override Color RemoteTargetingLineOutline => new("2e4a57ff");
}
