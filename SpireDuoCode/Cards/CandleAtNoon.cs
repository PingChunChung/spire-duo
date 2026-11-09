namespace SpireDuo.SpireDuoCode.Cards;
public sealed class CandleAtNoon() : BlackCatCard(1, CardType.Power, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CandleAtNoonPower>(3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<CandleAtNoonPower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<CandleAtNoonPower>(c, Owner.Creature, DynamicVars["CandleAtNoonPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["CandleAtNoonPower"].UpgradeValueBy(1m);
}
