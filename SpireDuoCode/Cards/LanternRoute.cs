using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class LanternRoute() : RaccoonCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(16m, ValueProp.Move), new BlockVar(10m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await RaccoonCardEffects.Hit(c, p, DynamicVars.Damage.BaseValue);
        if (await RaccoonStashStatePower.StashTopDraw(c, Owner, this))
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.BaseValue +(IsUpgraded? 3m : 0m), ValueProp.Move, p);
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(4m);
}
