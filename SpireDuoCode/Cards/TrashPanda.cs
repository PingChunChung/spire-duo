using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class TrashPanda() : RaccoonCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        int count = Owner.Creature.GetPower<RaccoonStashStatePower>()?.Count ?? 0;
        for (int i = 0; i < 2 + count; i++)
            await RaccoonCardEffects.Hit(c, p, DynamicVars.Damage.BaseValue +(IsUpgraded? 1m : 0m));
    }
    protected override void OnUpgrade()
    {
    }
}
