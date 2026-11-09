namespace SpireDuo.SpireDuoCode.Cards;
public sealed class ReturnToSender() : PandaCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ReturnToSenderPower>(50m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) => await PowerCmd.Apply<ReturnToSenderPower>(choiceContext, Owner.Creature, DynamicVars["ReturnToSenderPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["ReturnToSenderPower"].UpgradeValueBy(20m);
}
