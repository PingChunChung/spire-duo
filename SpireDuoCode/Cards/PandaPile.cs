namespace SpireDuo.SpireDuoCode.Cards;

public sealed class PandaPile() : PandaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PlatingPower>(3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PlatingPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        decimal amount = DynamicVars["PlatingPower"].BaseValue;
        await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, amount, Owner.Creature, this);
        await PowerCmd.Apply<PlatingPower>(choiceContext, cardPlay.Target, amount, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["PlatingPower"].UpgradeValueBy(1m);
    }
}
