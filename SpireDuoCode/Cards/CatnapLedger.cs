using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class CatnapLedger() : BlackCatCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CatnapLedgerPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<CatnapLedgerPower>()];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) => await PowerCmd.Apply<CatnapLedgerPower>(context, Owner.Creature, DynamicVars["CatnapLedgerPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["CatnapLedgerPower"].UpgradeValueBy(1m);
}
