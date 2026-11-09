namespace SpireDuo.SpireDuoCode.Cards;

public sealed class RudeAwakening() : KoalaCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Per", 4m), new EnergyVar(1), new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), EnergyHoverTip];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int removed = await DrowsyPower.WakeUp(choiceContext, Owner.Creature);
        int times = removed / DynamicVars["Per"].IntValue;
        if (times <= 0) return;
        await PlayerCmd.GainEnergy(times * DynamicVars.Energy.BaseValue, Owner);
        await CardPileCmd.Draw(choiceContext, times * DynamicVars.Cards.BaseValue, Owner);
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
