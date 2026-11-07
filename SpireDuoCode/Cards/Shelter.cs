namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Shelter() : PandaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override bool GainsBlock => true;
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5m, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        decimal block = Owner.Creature.Block;
        if (block > 0)
            await CreatureCmd.GainBlock(cardPlay.Target, block, ValueProp.Unpowered, cardPlay);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}
