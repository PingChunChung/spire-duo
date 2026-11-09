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

