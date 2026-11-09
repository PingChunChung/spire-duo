using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class NightAtlas() : BlackCatCard(3, CardType.Power, CardRarity.Ancient, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<NightAtlasPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<NightAtlasPower>()];

    protected override Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay) =>
        PowerCmd.Apply<NightAtlasPower>(context, Owner.Creature, DynamicVars["NightAtlasPower"].BaseValue, Owner.Creature, this);

    protected override void OnUpgrade() => DynamicVars["NightAtlasPower"].UpgradeValueBy(4m);
}
