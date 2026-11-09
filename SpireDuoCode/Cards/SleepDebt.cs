namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SleepDebt() : KoalaCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrowsyPower>(3m), new BlockVar(5m, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.Static(StaticHoverTip.Block)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature.Block <= 0)
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<DrowsyPower>(choiceContext, Owner.Creature, DynamicVars["DrowsyPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DrowsyPower"].UpgradeValueBy(1m);
        DynamicVars.Block.UpgradeValueBy(2m);
    }
}
