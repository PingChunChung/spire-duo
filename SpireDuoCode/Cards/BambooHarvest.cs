namespace SpireDuo.SpireDuoCode.Cards;

/// <summary>What Archaic Tooth turns <see cref="GatherBamboo"/> into.</summary>
public sealed class BambooHarvest() : PandaCard(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    public override bool GainsBlock => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(15m, ValueProp.Move), new PowerVar<BambooPower>(6m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<BambooPower>(choiceContext, Owner.Creature, DynamicVars["BambooPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(5m);
        DynamicVars["BambooPower"].UpgradeValueBy(2m);
    }
}
