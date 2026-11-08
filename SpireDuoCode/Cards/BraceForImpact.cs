namespace SpireDuo.SpireDuoCode.Cards;

public sealed class BraceForImpact() : PandaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CalculationBaseVar(5m), new CalculationExtraVar(5m),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((card, _) => card.CombatState?.HittableEnemies.Count(e => e.Monster?.IntendsToAttack ?? false) ?? 0)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), DynamicVars.CalculatedBlock.Props, cardPlay);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(3m);
    }
}
