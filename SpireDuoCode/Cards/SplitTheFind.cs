using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SplitTheFind() : RaccoonCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await RaccoonCardEffects.Hit(c, p, DynamicVars.Damage.BaseValue);
        if (p.Target?.Player != null && await RaccoonStashStatePower.Spend(c, Owner.Creature, this) != null)
            await CardPileCmd.Draw(c, IsUpgraded? 2 : 1, p.Target.Player);
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m);
}
