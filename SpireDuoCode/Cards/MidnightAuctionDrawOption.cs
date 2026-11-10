using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;
[Pool(typeof(TokenCardPool))]
public sealed class MidnightAuctionDrawOption() : SpireDuoCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1m);
}
