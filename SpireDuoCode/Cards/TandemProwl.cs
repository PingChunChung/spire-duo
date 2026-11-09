using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class TandemProwl() : BlackCatCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<TandemProwlPower>(5m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<TandemProwlPower>()];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) => await PowerCmd.Apply<TandemProwlPower>(context, Owner.Creature, DynamicVars["TandemProwlPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["TandemProwlPower"].UpgradeValueBy(3m);
}
