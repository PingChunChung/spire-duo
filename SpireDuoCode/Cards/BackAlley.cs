using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class BackAlley() : RaccoonCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        int count = Owner.Creature.GetPower<RaccoonStashStatePower>()?.Count ?? 0;
        await RaccoonCardEffects.Hit(c, p, DynamicVars.Damage.BaseValue + count * 2m);
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2m);
}
