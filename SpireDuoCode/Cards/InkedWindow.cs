using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class InkedWindow() : BlackCatCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<InkedWindowPower>(7m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<InkedWindowPower>()];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) => await PowerCmd.Apply<InkedWindowPower>(context, Owner.Creature, DynamicVars["InkedWindowPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["InkedWindowPower"].UpgradeValueBy(3m);
}
