using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class NightSpoils() : RaccoonCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override bool HasEnergyCostX => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        int energy = ResolveEnergyXValue();
        decimal damage = DynamicVars.Damage.BaseValue +(Owner.Creature.GetPower<RaccoonStashStatePower>()?.SpentThisTurn == true? 2m : 0m);
        for (int i = 0; i < energy; i++)
            await RaccoonCardEffects.Hit(c, p, damage +(IsUpgraded? 1m : 0m));
    }
    protected override void OnUpgrade()
    {
    }
}
