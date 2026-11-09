using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Scrupulous() : BlackCatCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ScrupulousPower>(2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ScrupulousPower>()];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) => await PowerCmd.Apply<ScrupulousPower>(context, Owner.Creature, DynamicVars["ScrupulousPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["ScrupulousPower"].UpgradeValueBy(1m);
}
