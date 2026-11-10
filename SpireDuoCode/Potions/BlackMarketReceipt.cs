using MegaCrit.Sts2.Core.Entities.Potions;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Potions;

public sealed class BlackMarketReceipt : RaccoonPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;
    protected override async Task OnUse(PlayerChoiceContext context, Creature? target)
    {
        var card = await RaccoonStashStatePower.Unstash(context, Owner.Creature, null!);
        if (card == null) return;
        card.EnergyCost.SetThisTurn(0);
        card.ExhaustOnNextPlay = true;
        await CardCmd.AutoPlay(context, card, null, AutoPlayType.Default);
    }
}
