namespace SpireDuo.SpireDuoCode.Cards;

/// <summary>What Archaic Tooth turns <see cref="Doze"/> into.</summary>
public sealed class DeepSleep() : KoalaCard(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    public override bool GainsBlock => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(15m, ValueProp.Move), new PowerVar<DrowsyPower>(6m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<DrowsyPower>(choiceContext, Owner.Creature, DynamicVars["DrowsyPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(5m);
        DynamicVars["DrowsyPower"].UpgradeValueBy(2m);
    }
}
