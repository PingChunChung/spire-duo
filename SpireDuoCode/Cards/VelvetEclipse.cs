namespace SpireDuo.SpireDuoCode.Cards;
public sealed class VelvetEclipse() : BlackCatCard(1, CardType.Power, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VelvetEclipsePower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VelvetEclipsePower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<VelvetEclipsePower>(c, Owner.Creature, 1, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["VelvetEclipsePower"].UpgradeValueBy(1m);
}
