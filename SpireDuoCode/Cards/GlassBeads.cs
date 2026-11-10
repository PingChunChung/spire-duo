using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class GlassBeads() : RaccoonCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<GlassBeadsPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<GlassBeadsPower>()];
    protected override Task OnPlay(PlayerChoiceContext c, CardPlay p) => PowerCmd.Apply<GlassBeadsPower>(c, Owner.Creature, 1, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["GlassBeadsPower"].UpgradeValueBy(1m);
}
