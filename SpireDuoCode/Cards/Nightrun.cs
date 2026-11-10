using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Nightrun() : RaccoonCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await RaccoonCardEffects.Hit(c, p, DynamicVars.Damage.BaseValue);
        if (Owner.Creature.GetPower<RaccoonStashStatePower>()?.Count > 0)
            await CardPileCmd.Draw(c, 1, Owner);
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m);
}
