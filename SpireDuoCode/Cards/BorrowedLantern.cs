using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class BorrowedLantern() : RaccoonCard(1, CardType.Power, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BorrowedLanternPower>(3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BorrowedLanternPower>()];
    protected override Task OnPlay(PlayerChoiceContext c, CardPlay p) => PowerCmd.Apply<BorrowedLanternPower>(c, Owner.Creature, DynamicVars["BorrowedLanternPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["BorrowedLanternPower"].UpgradeValueBy(1m);
}
