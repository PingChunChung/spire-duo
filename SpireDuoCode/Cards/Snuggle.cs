namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Snuggle() : KoalaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override bool GainsBlock => true;
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CalculationBaseVar(0m), new CalculationExtraVar(1m),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((card, _) => card.Owner.Creature.GetPowerAmount<DrowsyPower>())];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.Static(StaticHoverTip.Block)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        decimal block = DynamicVars.CalculatedBlock.Calculate(cardPlay.Target);
        if (block <= 0) return;
        await CreatureCmd.GainBlock(Owner.Creature, block, DynamicVars.CalculatedBlock.Props, cardPlay);
        await CreatureCmd.GainBlock(cardPlay.Target, block, DynamicVars.CalculatedBlock.Props, cardPlay);
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
