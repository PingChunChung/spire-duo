using MegaCrit.Sts2.Core.Entities.Potions;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Potions;

public sealed class CoinRattle : RaccoonPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;
    protected override async Task OnUse(PlayerChoiceContext context, Creature? target)
    {
        await RaccoonStashStatePower.StashFromHand(context, Owner, 1, null!);
        await CardPileCmd.Draw(context, 1, Owner);
    }
}
