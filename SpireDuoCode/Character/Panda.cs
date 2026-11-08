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

public class Panda : PlaceholderCharacterModel
{
    public const string CharacterId = "Panda";

    public static readonly Color Color = new("d8d4cc");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 80;

    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikePanda>(),
        ModelDb.Card<StrikePanda>(),
        ModelDb.Card<StrikePanda>(),
        ModelDb.Card<StrikePanda>(),
        ModelDb.Card<DefendPanda>(),
        ModelDb.Card<DefendPanda>(),
        ModelDb.Card<DefendPanda>(),
        ModelDb.Card<DefendPanda>(),
        ModelDb.Card<GatherBamboo>(),
        ModelDb.Card<HoldHands>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<BambooBasket>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<PandaCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<PandaRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<PandaPotionPool>();

    public override NCreatureVisuals? CreateCustomVisuals() =>
        NodeFactory<NCreatureVisuals>.CreateFromResource("panda_body.png".CharacterUiPath());

    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    public override string CustomIconTexturePath => "character_icon_panda.png".CharacterUiPath();
    public override string CustomIconOutlineTexturePath => "character_icon_panda_outline.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_panda.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_panda_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_panda.png".CharacterUiPath();

    // Everything below replaces what PlaceholderCharacterModel would otherwise borrow from the Ironclad.
    public override CustomEnergyCounter? CustomEnergyCounter =>
        new CustomEnergyCounter(i => $"orb/panda_orb_layer_{i}.png".CharacterUiPath(), new Color("2d4a12ff"), new Color("c8f07a"));
    public override string? CustomEnergyCounterPath => null;
    public override string CustomRestSiteAnimPath => "panda_rest_site.tscn".ScenePath();
    public override string CustomMerchantAnimPath => "panda_merchant.tscn".ScenePath();
    public override string CustomArmPointingTexturePath => "hands/panda_hand_point.png".CharacterUiPath();
    public override string CustomArmRockTexturePath => "hands/panda_hand_rock.png".CharacterUiPath();
    public override string CustomArmPaperTexturePath => "hands/panda_hand_paper.png".CharacterUiPath();
    public override string CustomArmScissorsTexturePath => "hands/panda_hand_scissors.png".CharacterUiPath();
    public override string CustomCharacterSelectBg => "panda_char_select_bg.tscn".ScenePath();
    public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/fade_transition_mat.tres";
    public override string CustomTrailPath => SceneHelper.GetScenePath("vfx/card_trail_silent");
    public override string CharacterSelectSfx => "event:/sfx/ui/enchant_shimmer";
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_map";

    // Co-op markers (map drawings, pings, targeting line, energy label) default to black for every modded character.
    public override Color EnergyLabelOutlineColor => new("2d4a12ff");
    public override Color DialogueColor => new("34421f");
    public override VfxColor SpeechBubbleColor => VfxColor.Green;
    public override Color MapDrawingColor => new("5a7f1e");
    public override Color RemoteTargetingLineColor => new("b5d96bff");
    public override Color RemoteTargetingLineOutline => new("2d4a12ff");
}
