namespace SpireDuo.SpireDuoCode.Cards;

public sealed class AllYouCanEat() : PandaCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Per", 3m), new EnergyVar(1), new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>(), EnergyHoverTip];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int eaten = await BambooPower.MunchAll(choiceContext, Owner.Creature, this);
        int times = eaten / DynamicVars["Per"].IntValue;
        if (times <= 0) return;
        await PlayerCmd.GainEnergy(times * DynamicVars.Energy.BaseValue, Owner);
        await CardPileCmd.Draw(choiceContext, times * DynamicVars.Cards.BaseValue, Owner);
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
