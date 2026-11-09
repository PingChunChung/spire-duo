namespace SpireDuo.SpireDuoCode.Cards;
public sealed class AlphaPanda() : PandaCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<AlphaPandaPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<AlphaPandaPower>(c, Owner.Creature, DynamicVars["AlphaPandaPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["AlphaPandaPower"].UpgradeValueBy(1m);
}
