using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class LanternShadow() : BlackCatCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<LanternShadowPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<LanternShadowPower>()];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) => await PowerCmd.Apply<LanternShadowPower>(context, Owner.Creature, DynamicVars["LanternShadowPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["LanternShadowPower"].UpgradeValueBy(1m);
}
