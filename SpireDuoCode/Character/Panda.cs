using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using SpireDuo.SpireDuoCode.Cards;
using SpireDuo.SpireDuoCode.Extensions;
using SpireDuo.SpireDuoCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;

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
    public override string CustomCharacterSelectIconPath => "char_select_panda.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_panda_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_panda.png".CharacterUiPath();
}
