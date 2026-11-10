using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DirtyWork() : RaccoonCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        int count = await RaccoonStashStatePower.SpendUpTo(c, Owner.Creature, this, 2);
        for (int i = 0; i < count; i++)
            await RaccoonCardEffects.Hit(c, p, DynamicVars.Damage.BaseValue +(IsUpgraded? 2m : 0m));
    }
    protected override void OnUpgrade()
    {
    }
}
