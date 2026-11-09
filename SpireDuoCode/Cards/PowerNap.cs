namespace SpireDuo.SpireDuoCode.Cards;

public sealed class PowerNap() : KoalaCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrowsyPower>(2m), new EnergyVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), EnergyHoverTip];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<DrowsyPower>(choiceContext, Owner.Creature, DynamicVars["DrowsyPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Energy.BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["DrowsyPower"].UpgradeValueBy(1m);
    }
}
