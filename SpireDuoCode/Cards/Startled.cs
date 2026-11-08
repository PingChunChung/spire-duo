namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Startled() : KoalaCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.Static(StaticHoverTip.Block)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int removed = await DrowsyPower.WakeUp(choiceContext, Owner.Creature);
        if (removed > 0)
            await CreatureCmd.GainBlock(Owner.Creature, removed, ValueProp.Move, cardPlay);
        if (IsUpgraded)
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }
    protected override void OnUpgrade()
    {
    }
}
