namespace SpireDuo.SpireDuoCode.Cards;
public sealed class BlackThread() : BlackCatCard(1, CardType.Power, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BlackThreadPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BlackThreadPower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<BlackThreadPower>(c, Owner.Creature, 1, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["BlackThreadPower"].UpgradeValueBy(1m);
}
