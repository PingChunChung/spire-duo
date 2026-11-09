namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SlumberParty() : KoalaCard(2, CardType.Skill, CardRarity.Rare, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrowsyPower>(5m), new CardsVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target?.Player, "cardPlay.Target.Player");
        await PowerCmd.Apply<DrowsyPower>(choiceContext, cardPlay.Target, DynamicVars["DrowsyPower"].BaseValue, Owner.Creature, this);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, cardPlay.Target.Player);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["DrowsyPower"].UpgradeValueBy(2m);
    }
}
