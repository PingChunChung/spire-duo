using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Crowbar() : RaccoonCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        if (await RaccoonStashStatePower.Spend(c, Owner.Creature, this, x => RaccoonStashStatePower.CostValue(x) >= 2) != null)
            await RaccoonCardEffects.Hit(c, p, DynamicVars.Damage.BaseValue);
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(4m);
}
