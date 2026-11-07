using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using SpireDuo.SpireDuoCode.Character;
using SpireDuo.SpireDuoCode.Extensions;

namespace SpireDuo.SpireDuoCode.Potions;

[Pool(typeof(KoalaPotionPool))]
public abstract class SpireDuoPotion : CustomPotionModel
{
	public override string? CustomPackedImagePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
	public override string? CustomPackedOutlinePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionOutlineImagePath();
}