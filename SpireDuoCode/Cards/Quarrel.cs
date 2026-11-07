using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace SpireDuo.SpireDuoCode.Cards;

[Pool(typeof(ColorlessCardPool))]
public sealed class Quarrel() : SpireDuoCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(13m, ValueProp.Move), new HpLossVar(2m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
        // Every player pays for the argument.
        foreach (var ally in CombatState!.PlayerCreatures.Where(c => c.IsAlive).ToList())
        {
            await CreatureCmd.Damage(choiceContext, ally, DynamicVars.HpLoss.BaseValue,
                ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}
