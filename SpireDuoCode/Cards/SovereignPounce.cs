using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SovereignPounce() : BlackCatCard(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(26m, ValueProp.Move), new DynamicVar("PounceBonus", 3m)];

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Target == null)
            return;

        int pounces = Owner.Creature.GetPower<FootprintStatePower>()?.PouncesThisCombat ?? 0;
        decimal damage = DynamicVars.Damage.BaseValue + pounces * DynamicVars["PounceBonus"].BaseValue;
        await DamageCmd.Attack(damage).FromCard(this).Targeting(cardPlay.Target).Execute(context);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6m);
        DynamicVars["PounceBonus"].UpgradeValueBy(1m);
    }
}
