namespace SpireDuo.SpireDuoCode.Cards;

public sealed class CountingSheep() : KoalaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CalculationBaseVar(0m), new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedDrowsy").WithMultiplier((card, _) => PileType.Hand.GetPile(card.Owner).Cards.Count(c => c != card))];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int others = PileType.Hand.GetPile(Owner).Cards.Count(c => c != this);
        if (others > 0)
            await PowerCmd.Apply<DrowsyPower>(choiceContext, Owner.Creature, others * DynamicVars.CalculationExtra.BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}
