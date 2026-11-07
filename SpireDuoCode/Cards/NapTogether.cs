namespace SpireDuo.SpireDuoCode.Cards;

public sealed class NapTogether() : KoalaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrowsyPower>(3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        decimal amount = DynamicVars["DrowsyPower"].BaseValue;
        await PowerCmd.Apply<DrowsyPower>(choiceContext, Owner.Creature, amount, Owner.Creature, this);
        await PowerCmd.Apply<DrowsyPower>(choiceContext, cardPlay.Target, amount, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["DrowsyPower"].UpgradeValueBy(1m);
    }
}
