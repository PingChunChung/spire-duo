using MegaCrit.Sts2.Core.Factories;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class LucidDream() : KoalaCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = CardFactory.GetDistinctForCombat(Owner, Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
            .Where(c => (c.Rarity is CardRarity.Common or CardRarity.Uncommon or CardRarity.Rare) && c is not LucidDream),
            DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardGeneration).ToList();
        foreach (var card in cards)
        {
            card.AddKeyword(CardKeyword.Retain);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}
