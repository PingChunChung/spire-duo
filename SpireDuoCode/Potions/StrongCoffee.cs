using MegaCrit.Sts2.Core.Entities.Potions;

namespace SpireDuo.SpireDuoCode.Potions;

public sealed class StrongCoffee : KoalaPotion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AllEnemies;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("PerDrowsy", 2m)];

    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        var creature = Owner.Creature;
        int removed = await DrowsyPower.WakeUp(choiceContext, creature);
        if (removed <= 0) return;
        var enemies = creature.CombatState!.HittableEnemies.ToList();
        if (enemies.Count == 0) return;
        await CreatureCmd.Damage(choiceContext, enemies, removed * DynamicVars["PerDrowsy"].BaseValue, ValueProp.Unpowered, creature, null);
    }
}
