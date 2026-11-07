using MegaCrit.Sts2.Core.Entities.Potions;

namespace SpireDuo.SpireDuoCode.Potions;

public sealed class HoneyJar : PandaPotion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(12m, ValueProp.Unpowered)];

    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block), HoverTipFactory.FromPower<BlurPower>()];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        PotionModel.AssertValidForTargetedPotion(target);
        await CreatureCmd.GainBlock(target!, DynamicVars.Block, null);
        await PowerCmd.Apply<BlurPower>(choiceContext, target!, 1m, Owner.Creature, null);
    }
}
