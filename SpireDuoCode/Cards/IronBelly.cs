namespace SpireDuo.SpireDuoCode.Cards;

public sealed class IronBelly() : PandaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CalculationBaseVar(5m), new CalculationExtraVar(2m),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((card, _) => Math.Max(0, card.Owner.Creature.GetPowerAmount<StrengthPower>()))];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), DynamicVars.CalculatedBlock.Props, cardPlay);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(3m);
    }
}
