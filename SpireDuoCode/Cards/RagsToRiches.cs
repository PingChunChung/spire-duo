using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class RagsToRiches() : RaccoonCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<RagsToRichesPower>(8m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<RagsToRichesPower>()];
    protected override Task OnPlay(PlayerChoiceContext c, CardPlay p) => PowerCmd.Apply<RagsToRichesPower>(c, Owner.Creature, DynamicVars["RagsToRichesPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["RagsToRichesPower"].UpgradeValueBy(3m);
}
