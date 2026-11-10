using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class RooftopSignal() : RaccoonCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9m, ValueProp.Move), new DynamicVar("Bonus", 8m)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await RaccoonCardEffects.Hit(c, p, DynamicVars.Damage.BaseValue);
        // The ally-next-Attack bonus is represented by the dedicated co-op power.
        var ally = RaccoonStashStatePower.DeterministicAlly(Owner.Creature);
        if (ally != null)
            await PowerCmd.Apply<PincerPower>(c, ally.Creature, DynamicVars["Bonus"].BaseValue +(IsUpgraded? 2m : 0m), Owner.Creature, this);
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m);
}
