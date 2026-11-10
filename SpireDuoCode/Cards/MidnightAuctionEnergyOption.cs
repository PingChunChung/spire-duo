using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;
[Pool(typeof(TokenCardPool))]
public sealed class MidnightAuctionEnergyOption() : SpireDuoCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [EnergyHoverTip];
    protected override void OnUpgrade() => DynamicVars.Energy.UpgradeValueBy(1m);
}
