using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class RaccoonRaid() : RaccoonCard(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        int count = await RaccoonStashStatePower.SpendAll(c, Owner.Creature, this);
        for (int i = 0; i < count; i++)
            await RaccoonCardEffects.Hit(c, p, DynamicVars.Damage.BaseValue);
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2m);
}
