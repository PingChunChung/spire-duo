using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Redirection() : RaccoonCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9m, ValueProp.Move), new PowerVar<WeakPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        var spent = await RaccoonStashStatePower.Spend(c, Owner.Creature, this);
        await RaccoonCardEffects.Hit(c, p, DynamicVars.Damage.BaseValue);
        if (p.Target != null && spent != null && spent.Type == CardType.Attack)
            await PowerCmd.Apply<WeakPower>(c, p.Target, DynamicVars["WeakPower"].BaseValue +(IsUpgraded? 1m : 0m), Owner.Creature, this);
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m);
}
