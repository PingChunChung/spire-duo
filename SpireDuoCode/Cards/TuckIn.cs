namespace SpireDuo.SpireDuoCode.Cards;

public sealed class TuckIn() : KoalaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CalculationBaseVar(0m), new CalculationExtraVar(2m),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((card, _) => PileType.Hand.GetPile(card.Owner).Cards.Count(c => c != card))];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), DynamicVars.CalculatedBlock.Props, cardPlay);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.CalculationExtra.UpgradeValueBy(1m);
    }
}
