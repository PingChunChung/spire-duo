using BaseLib.Abstracts;
using SpireDuo.SpireDuoCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using SpireDuo.SpireDuoCode.Cards;

namespace SpireDuo.SpireDuoCode.Enchantments;

/// <summary>Marker read by BambooPower after its enchanted card successfully Munches.</summary>
public sealed class FreshlyCut : CustomEnchantmentModel
{
    protected override string CustomIconPath => "freshly_cut.png".EnchantmentImagePath();

    public override bool CanEnchant(CardModel card) => base.CanEnchant(card)
        && card.Owner.Character is Character.Panda
        && MunchCardTypes.Contains(card.GetType());

    private static readonly HashSet<Type> MunchCardTypes =
    [
        typeof(AllYouCanEat),
        typeof(BambooCharge),
        typeof(BambooProtein),
        typeof(BambooStick),
        typeof(BambooTornado),
        typeof(BearHug),
        typeof(BellyBump),
        typeof(BodyRoll),
        typeof(Chomp),
        typeof(Forage),
        typeof(Nibble),
        typeof(PandaPower),
        typeof(PawCombo),
        typeof(RolyPoly),
        typeof(SecondCourse),
        typeof(ShareBamboo),
        typeof(Snowball),
        typeof(TasteTest),
        typeof(ThousandPaws),
        typeof(Tumble),
    ];
}
