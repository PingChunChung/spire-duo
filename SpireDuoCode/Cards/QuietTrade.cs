using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class QuietTrade() : RaccoonCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<QuietTradePower>(4m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<QuietTradePower>()];
    protected override Task OnPlay(PlayerChoiceContext c, CardPlay p) => PowerCmd.Apply<QuietTradePower>(c, Owner.Creature, DynamicVars["QuietTradePower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["QuietTradePower"].UpgradeValueBy(2m);
}
