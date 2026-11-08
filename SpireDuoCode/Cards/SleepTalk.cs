using MegaCrit.Sts2.Core.Factories;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SleepTalk() : KoalaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = CardFactory.GetDistinctForCombat(Owner, Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
            .Where(c => (c.Rarity is CardRarity.Common or CardRarity.Uncommon or CardRarity.Rare) && c is not (SleepTalk or LucidDream)),
            1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
        if (card == null) return;
        card.SetToFreeThisTurn();
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
