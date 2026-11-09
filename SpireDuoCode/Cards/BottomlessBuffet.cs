namespace SpireDuo.SpireDuoCode.Cards;
public sealed class BottomlessBuffet() : PandaCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BottomlessBuffetPower>(2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>(), HoverTipFactory.FromPower<StrengthPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) => await PowerCmd.Apply<BottomlessBuffetPower>(choiceContext, Owner.Creature, DynamicVars["BottomlessBuffetPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["BottomlessBuffetPower"].UpgradeValueBy(1m);
}
