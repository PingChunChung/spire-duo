namespace SpireDuo.SpireDuoCode.Cards;

public sealed class WakeUpKiss() : KoalaCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new EnergyVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), EnergyHoverTip];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DrowsyPower.WakeUp(choiceContext, Owner.Creature);
        var ally = cardPlay.Target.Player;
        if (ally == null) return;
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, ally);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, ally);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}
