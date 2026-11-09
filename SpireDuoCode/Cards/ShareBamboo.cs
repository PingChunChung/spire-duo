namespace SpireDuo.SpireDuoCode.Cards;

public sealed class ShareBamboo() : PandaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override bool GainsBlock => true;
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8m, ValueProp.Move), new PowerVar<StrengthPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>(), HoverTipFactory.FromPower<StrengthPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await CreatureCmd.GainBlock(cardPlay.Target, DynamicVars.Block, cardPlay);
        if (await BambooPower.Munch(choiceContext, Owner.Creature, 2, this))
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, DynamicVars["StrengthPower"].BaseValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}
