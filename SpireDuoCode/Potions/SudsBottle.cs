using MegaCrit.Sts2.Core.Entities.Potions;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Potions;

public sealed class SudsBottle : RaccoonPotion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(10m, ValueProp.Unpowered)];
    protected override async Task OnUse(PlayerChoiceContext context, Creature? target)
    {
        await RaccoonStashStatePower.Spend(context, Owner.Creature, null!);
        foreach (var power in Owner.Creature.Powers.Where(power => power.Type == PowerType.Debuff).ToArray()) await PowerCmd.Remove(power);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null);
    }
}
