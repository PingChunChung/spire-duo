namespace SpireDuo.SpireDuoCode.Cards;

public sealed class BackToSleep() : KoalaCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrowsyPower>(3m), new DynamicVar("Bonus", 3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];
    protected override bool ShouldGlowGoldInternal => Owner.Creature.GetPowerAmount<DrowsyPower>() == 0;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal amount = DynamicVars["DrowsyPower"].BaseValue;
        if (Owner.Creature.GetPowerAmount<DrowsyPower>() == 0) amount += DynamicVars["Bonus"].BaseValue;
        await PowerCmd.Apply<DrowsyPower>(choiceContext, Owner.Creature, amount, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["DrowsyPower"].UpgradeValueBy(1m);
        DynamicVars["Bonus"].UpgradeValueBy(1m);
    }
}
