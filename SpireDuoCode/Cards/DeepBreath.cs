namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DeepBreath() : PandaCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal block = Owner.Creature.Block;
        if (block > 0)
            await CreatureCmd.GainBlock(Owner.Creature, block, ValueProp.Unpowered, cardPlay);
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
