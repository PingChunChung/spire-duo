namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DoubleUp() : PandaCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int amount = Owner.Creature.GetPowerAmount<BambooPower>();
        if (amount > 0)
            await PowerCmd.Apply<BambooPower>(choiceContext, Owner.Creature, amount, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}
