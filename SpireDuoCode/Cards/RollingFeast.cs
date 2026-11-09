namespace SpireDuo.SpireDuoCode.Cards;
public sealed class RollingFeast() : PandaCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<RollingFeastPower>(2m), new CardsVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>(), HoverTipFactory.FromPower<RollPower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<RollingFeastPower>(c, Owner.Creature, DynamicVars["RollingFeastPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() { DynamicVars["RollingFeastPower"].UpgradeValueBy(1m); DynamicVars.Cards.UpgradeValueBy(1m); }
}
