using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace SpireDuo.SpireDuoCode.Cards;

/// <summary>
/// Sets up an ally: their next hit on the target this turn deals extra damage. Vanilla Gang Up rewards attacking after
/// allies; this one rewards attacking first.
/// </summary>
[Pool(typeof(ColorlessCardPool))]
public sealed class Pincer() : SpireDuoCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move), new PowerVar<PincerPower>(7m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PincerPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        if (cardPlay.Target.IsAlive)
            await PowerCmd.Apply<PincerPower>(choiceContext, cardPlay.Target, DynamicVars["PincerPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["PincerPower"].UpgradeValueBy(2m);
    }
}
