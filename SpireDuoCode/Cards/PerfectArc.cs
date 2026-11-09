using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class PerfectArc() : BlackCatCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    private const decimal PounceDamage = 8m;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10m, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Target == null)
            return;

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).Execute(context);
        if (Owner.Creature.GetPower<FootprintStatePower>()?.HasTriggered(FootprintLine.Pounce) == true)
            await DamageCmd.Attack(PounceDamage).FromCard(this).Targeting(cardPlay.Target).Execute(context);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m);
}
