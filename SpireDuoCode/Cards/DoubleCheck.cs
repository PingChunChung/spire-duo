using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DoubleCheck() : RaccoonCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        var cards = Owner.Creature.GetPower<RaccoonStashStatePower>()?.Slots ?? [];
        if (cards.Any(card => card.Type == CardType.Attack) && cards.Any(card => card.Type == CardType.Skill) && cards.Any(card => card.Type == CardType.Power))
            await CardPileCmd.Draw(c, 3, Owner);
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(- 1);
}
