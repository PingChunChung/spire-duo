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
    public override string CustomCharacterSelectIconPath => "char_select_koala.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_koala_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_koala.png".CharacterUiPath();
}
