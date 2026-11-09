using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Potions;
using SpireDuo.SpireDuoCode.Cards;

namespace SpireDuo.SpireDuoCode.Potions;

public sealed class PawprintDraught : BlackCatPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        var choices = new CardModel[] { ModelDb.Card<StrikeCat>(), ModelDb.Card<DefendCat>(), ModelDb.Card<BlackThread>() };
        CardModel? choice = await CardSelectCmd.FromChooseACardScreen(choiceContext, choices, Owner);
        Footprint footprint = choice?.Type switch
        {
            CardType.Attack => Footprint.Attack,
            CardType.Skill => Footprint.Skill,
            CardType.Power => Footprint.Power,
            _ => Footprint.Attack
        };
        var state = await FootprintStatePower.GetOrCreate(choiceContext, Owner.Creature);
        int slot = Enumerable.Range(0, 3).FirstOrDefault(index => state.Slots[index] == Footprint.Empty && !state.Locked[index]);
        await state.Set(choiceContext, slot, footprint, null);
        await CardPileCmd.Draw(choiceContext, 1, Owner);
    }
}

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

public sealed class NinthSpark : BlackCatPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        var choices = new CardModel[] { ModelDb.Card<StrikeCat>(), ModelDb.Card<DefendCat>(), ModelDb.Card<BlackThread>() };
        CardModel? choice = await CardSelectCmd.FromChooseACardScreen(choiceContext, choices, Owner);
        FootprintLine line = choice?.Type switch
        {
            CardType.Attack => FootprintLine.Pounce,
            CardType.Skill => FootprintLine.Groom,
            _ => FootprintLine.Omen
        };
        var state = await FootprintStatePower.GetOrCreate(choiceContext, Owner.Creature);
        await state.TriggerLineNow(choiceContext, new CardPlay
        {
            Card = ModelDb.Card<BlackThread>(),
            Target = null,
            ResultPile = PileType.Exhaust,
            Resources = default,
            IsAutoPlay = true,
            PlayIndex = 0,
            PlayCount = 1
        }, line);
    }
}
