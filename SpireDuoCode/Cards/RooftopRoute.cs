using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class RooftopRoute() : RaccoonCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9m, ValueProp.Move), new DynamicVar("Bonus", 7m)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await RaccoonCardEffects.Hit(c, p, DynamicVars.Damage.BaseValue);
        var stash = Owner.Creature.GetPower<RaccoonStashStatePower>();
        if (stash != null && stash.Count == stash.Capacity)
            await RaccoonCardEffects.Hit(c, p, DynamicVars["Bonus"].BaseValue +(IsUpgraded? 1m : 0m));
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2m);
}
