using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace SpireDuo.SpireDuoCode.Cards;

[Pool(typeof(ColorlessCardPool))]
public sealed class Promise() : SpireDuoCard(1, CardType.Power, CardRarity.Rare, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PromisePower>(3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PromisePower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        var amount = DynamicVars["PromisePower"].BaseValue;
        await PowerCmd.Apply<PromisePower>(choiceContext, Owner.Creature, amount, Owner.Creature, this);
        await PowerCmd.Apply<PromisePower>(choiceContext, cardPlay.Target, amount, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PromisePower"].UpgradeValueBy(1m);
    }
}
