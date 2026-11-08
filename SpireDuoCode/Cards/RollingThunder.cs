namespace SpireDuo.SpireDuoCode.Cards;

/// <summary>Panda's Dusty Tome card.</summary>
public sealed class RollingThunder() : PandaCard(3, CardType.Power, CardRarity.Ancient, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<RollingThunderPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<RollingThunderPower>(choiceContext, Owner.Creature, DynamicVars["RollingThunderPower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
