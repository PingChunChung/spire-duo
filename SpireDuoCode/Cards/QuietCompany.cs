using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class QuietCompany() : BlackCatCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<QuietCompanyPower>(2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<QuietCompanyPower>()];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) => await PowerCmd.Apply<QuietCompanyPower>(context, Owner.Creature, DynamicVars["QuietCompanyPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["QuietCompanyPower"].UpgradeValueBy(1m);
}
