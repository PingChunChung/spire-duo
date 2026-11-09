using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class MidnightOracle() : BlackCatCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<MidnightOraclePower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<MidnightOraclePower>()];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) => await PowerCmd.Apply<MidnightOraclePower>(context, Owner.Creature, DynamicVars["MidnightOraclePower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["MidnightOraclePower"].UpgradeValueBy(1m);
}
