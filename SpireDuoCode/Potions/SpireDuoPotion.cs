using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using SpireDuo.SpireDuoCode.Character;
using SpireDuo.SpireDuoCode.Extensions;

namespace SpireDuo.SpireDuoCode.Potions;

public abstract class SpireDuoPotion : CustomPotionModel
{
	public override string? CustomPackedImagePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
	public override string? CustomPackedOutlinePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionOutlineImagePath();
}

[Pool(typeof(KoalaPotionPool))]
public abstract class KoalaPotion : SpireDuoPotion
{
}

[Pool(typeof(PandaPotionPool))]
public abstract class PandaPotion : SpireDuoPotion
{
}
