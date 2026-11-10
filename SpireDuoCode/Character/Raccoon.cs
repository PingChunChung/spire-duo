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

public sealed class Raccoon : PlaceholderCharacterModel
{
    public const string CharacterId = "Raccoon";
    public static readonly Color Color = new("c96f35");
    public override bool HideFromVanillaCharacterSelect => true;
    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 74;
    public override IEnumerable<CardModel> StartingDeck => [ModelDb.Card<StrikeRaccoon>(), ModelDb.Card<StrikeRaccoon>(), ModelDb.Card<StrikeRaccoon>(), ModelDb.Card<StrikeRaccoon>(), ModelDb.Card<DefendRaccoon>(), ModelDb.Card<DefendRaccoon>(), ModelDb.Card<DefendRaccoon>(), ModelDb.Card<DefendRaccoon>(), ModelDb.Card<PocketLining>(), ModelDb.Card<HoldHands>()];
    public override IReadOnlyList<RelicModel> StartingRelics => [ModelDb.Relic<CanvasSatchel>()];
    public override CardPoolModel CardPool => ModelDb.CardPool<RaccoonCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<RaccoonRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<RaccoonPotionPool>();
    public override NCreatureVisuals? CreateCustomVisuals() => NodeFactory<NCreatureVisuals>.CreateFromResource("raccoon_body.png".CharacterUiOrKoalaPath("koala_body.png"));
    public override Control CustomIcon => NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
    public override string CustomIconTexturePath => "character_icon_raccoon.png".CharacterUiOrKoalaPath("character_icon_koala.png");
    public override string CustomIconOutlineTexturePath => "character_icon_raccoon_outline.png".CharacterUiOrKoalaPath("character_icon_koala_outline.png");
    public override string CustomCharacterSelectIconPath => "char_select_raccoon.png".CharacterUiOrKoalaPath("char_select_koala.png");
    public override string CustomCharacterSelectLockedIconPath => "char_select_raccoon_locked.png".CharacterUiOrKoalaPath("char_select_koala_locked.png");
    public override string? CustomMapMarkerPath => "map_marker_raccoon.png".CharacterUiOrKoalaPath("map_marker_koala.png");
    public override CustomEnergyCounter? CustomEnergyCounter => new(i => $"orb/raccoon_orb_layer_{i}.png".CharacterUiOrKoalaPath($"orb/koala_orb_layer_{i}.png"), new Color("2b1a14"), new Color("f0b35a"));
    public override string CustomEnergyCounterPath => null!;
    public override string CustomRestSiteAnimPath => "raccoon_rest_site.tscn".SceneOrKoalaPath("koala_rest_site.tscn");
    public override string CustomMerchantAnimPath => "raccoon_merchant.tscn".SceneOrKoalaPath("koala_merchant.tscn");
    public override string CustomArmPointingTexturePath => "hands/raccoon_hand_point.png".CharacterUiOrKoalaPath("hands/koala_hand_point.png");
    public override string CustomArmRockTexturePath => "hands/raccoon_hand_rock.png".CharacterUiOrKoalaPath("hands/koala_hand_rock.png");
    public override string CustomArmPaperTexturePath => "hands/raccoon_hand_paper.png".CharacterUiOrKoalaPath("hands/koala_hand_paper.png");
    public override string CustomArmScissorsTexturePath => "hands/raccoon_hand_scissors.png".CharacterUiOrKoalaPath("hands/koala_hand_scissors.png");
    public override string CustomCharacterSelectBg => "raccoon_char_select_bg.tscn".SceneOrKoalaPath("koala_char_select_bg.tscn");
    public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/fade_transition_mat.tres";
    public override string CustomTrailPath => SceneHelper.GetScenePath("vfx/card_trail_ironclad");
    public override string CharacterSelectSfx => "event:/sfx/ui/coin_flip";
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_map";
    public override Color EnergyLabelOutlineColor => new("3a2117");
    public override Color DialogueColor => new("3a2117");
    public override VfxColor SpeechBubbleColor => VfxColor.Orange;
    public override Color MapDrawingColor => new("a84e28");
    public override Color RemoteTargetingLineColor => new("f0a45d");
    public override Color RemoteTargetingLineOutline => new("3a2117");
}
