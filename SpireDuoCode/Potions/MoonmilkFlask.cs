using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Potions;
using SpireDuo.SpireDuoCode.Cards;

namespace SpireDuo.SpireDuoCode.Potions;

public sealed class MoonmilkFlask : BlackCatPotion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(12m, ValueProp.Unpowered)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null);
        await FootprintStatePower.Lock(choiceContext, Owner.Creature, 0);
    }
}

