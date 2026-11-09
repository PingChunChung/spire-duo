namespace SpireDuo.SpireDuoCode.Cards;

public sealed class TuckAndRoll() : PandaCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5m, ValueProp.Move), new PowerVar<RollPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block), HoverTipFactory.FromPower<RollPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await RollPower.Gain(choiceContext, Owner.Creature, DynamicVars["RollPower"].BaseValue, this);
    }
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3m);
}
