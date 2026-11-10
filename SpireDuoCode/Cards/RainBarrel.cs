using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class RainBarrel() : RaccoonCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<RainBarrelPower>(4m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<RainBarrelPower>()];
    protected override Task OnPlay(PlayerChoiceContext c, CardPlay p) => PowerCmd.Apply<RainBarrelPower>(c, Owner.Creature, DynamicVars["RainBarrelPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["RainBarrelPower"].UpgradeValueBy(1m);
}
