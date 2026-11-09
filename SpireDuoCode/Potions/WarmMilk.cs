using MegaCrit.Sts2.Core.Entities.Potions;

namespace SpireDuo.SpireDuoCode.Potions;

public sealed class WarmMilk : KoalaPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrowsyPower>(3m)];

    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        PotionModel.AssertValidForTargetedPotion(target);
        await PowerCmd.Apply<DrowsyPower>(choiceContext, target!, DynamicVars["DrowsyPower"].BaseValue, Owner.Creature, null);
    }
}
