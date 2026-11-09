namespace SpireDuo.SpireDuoCode.Cards;

public sealed class BambooWall() : PandaCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CalculationBaseVar(5m), new CalculationExtraVar(1m),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((card, _) => Math.Min(8, card.Owner.Creature.GetPowerAmount<BambooPower>() / 2))];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>(), HoverTipFactory.Static(StaticHoverTip.Block)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), DynamicVars.CalculatedBlock.Props, cardPlay);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(3m);
    }
}
