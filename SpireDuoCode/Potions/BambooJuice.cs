using MegaCrit.Sts2.Core.Entities.Potions;

namespace SpireDuo.SpireDuoCode.Potions;

public sealed class BambooJuice : PandaPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BambooPower>(6m)];

    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        PotionModel.AssertValidForTargetedPotion(target);
        await PowerCmd.Apply<BambooPower>(choiceContext, target!, DynamicVars["BambooPower"].BaseValue, Owner.Creature, null);
    }
}
