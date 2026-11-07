namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DreamShield() : KoalaCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Multiplier", 2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.Static(StaticHoverTip.Block)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int amount = Owner.Creature.GetPowerAmount<DrowsyPower>() * DynamicVars["Multiplier"].IntValue;
        if (amount > 0)
            await CreatureCmd.GainBlock(Owner.Creature, amount, ValueProp.Move, cardPlay);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Multiplier"].UpgradeValueBy(1m);
    }
}
