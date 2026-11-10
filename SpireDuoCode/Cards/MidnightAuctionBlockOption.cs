using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;
[Pool(typeof(TokenCardPool))]
public sealed class MidnightAuctionBlockOption() : SpireDuoCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(18m, ValueProp.Move)];
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3m);
}
