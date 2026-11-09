using MegaCrit.Sts2.Core.Entities.Potions;

namespace SpireDuo.SpireDuoCode.Potions;

public sealed class CrunchyShoot : PandaPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BambooPower>(8m), new PowerVar<CrunchyShootPower>(3m)];

    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        await PowerCmd.Apply<BambooPower>(choiceContext, Owner.Creature, DynamicVars["BambooPower"].BaseValue, Owner.Creature, null);
        await PowerCmd.Apply<CrunchyShootPower>(choiceContext, Owner.Creature, DynamicVars["CrunchyShootPower"].BaseValue, Owner.Creature, null);
    }
}
