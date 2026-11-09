using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Models.PotionPools;

namespace SpireDuo.SpireDuoCode.Potions;

/// <summary>Event-only potion, kept out of all normal character and shared pools.</summary>
[Pool(typeof(EventPotionPool))]
public sealed class HalfAndHalf : SpireDuoPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyAlly;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5m, ValueProp.Unpowered), new CardsVar(1)];

    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        PotionModel.AssertValidForTargetedPotion(target);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        await CreatureCmd.GainBlock(target!, DynamicVars.Block, null);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, target!.Player!);
    }
}
