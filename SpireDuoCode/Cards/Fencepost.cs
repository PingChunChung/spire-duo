using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Fencepost() : RaccoonCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move), new PowerVar<VulnerablePower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await RaccoonCardEffects.Hit(c, p, DynamicVars.Damage.BaseValue);
        if (p.Target != null && await RaccoonStashStatePower.Spend(c, Owner.Creature, this, x => x.Type == CardType.Skill) != null)
            await PowerCmd.Apply<VulnerablePower>(c, p.Target, 1, Owner.Creature, this);
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m);
}
