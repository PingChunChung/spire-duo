using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Potions;
using SpireDuo.SpireDuoCode.Cards;

namespace SpireDuo.SpireDuoCode.Potions;

public sealed class NinthSpark : BlackCatPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        // The choose screen only renders combat instances, not canonical models.
        var combat = Owner.Creature.CombatState!;
        var choices = new CardModel[] { combat.CreateCard<StrikeCat>(Owner), combat.CreateCard<DefendCat>(Owner), combat.CreateCard<BlackThread>(Owner) };
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
            Card = Owner.Creature.CombatState!.CreateCard<BlackThread>(Owner),
            Target = null,
            ResultPile = PileType.Exhaust,
            Resources = default,
            IsAutoPlay = true,
            PlayIndex = 0,
            PlayCount = 1
        }, line);
    }
}
