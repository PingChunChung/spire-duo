namespace SpireDuo.SpireDuoCode.Cards;

public sealed class FoodComa() : PandaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("PerBamboo", 4m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>(), HoverTipFactory.Static(StaticHoverTip.Block)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int eaten = await BambooPower.MunchAll(choiceContext, Owner.Creature, this);
        if (eaten > 0)
            await CreatureCmd.GainBlock(Owner.Creature, eaten * DynamicVars["PerBamboo"].BaseValue, ValueProp.Move, cardPlay);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PerBamboo"].UpgradeValueBy(1m);
    }
}
