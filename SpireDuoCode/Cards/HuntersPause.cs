using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class HuntersPause() : BlackCatCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<HuntersPausePower>(4m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<HuntersPausePower>()];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) => await PowerCmd.Apply<HuntersPausePower>(context, Owner.Creature, DynamicVars["HuntersPausePower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["HuntersPausePower"].UpgradeValueBy(2m);
}
