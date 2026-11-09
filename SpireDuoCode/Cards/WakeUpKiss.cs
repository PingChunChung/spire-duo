namespace SpireDuo.SpireDuoCode.Cards;

public sealed class WakeUpKiss() : KoalaCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrowsyPower>(4m), new CardsVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        bool woke = await DrowsyPower.WakeUp(choiceContext, Owner.Creature) > 0;
        var ally = cardPlay.Target.Player;
        if (ally == null) return;
        await PowerCmd.Apply<DrowsyPower>(choiceContext, ally.Creature, DynamicVars["DrowsyPower"].BaseValue, Owner.Creature, this);
        if (woke) await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, ally);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["DrowsyPower"].UpgradeValueBy(2m);
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}
