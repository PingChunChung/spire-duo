using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class CleansingSprint() : BlackCatCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(14m, ValueProp.Move), new DynamicVar("PerExhaust", 2m)];

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Target == null)
            return;

        int exhaustedCards = PileType.Exhaust.GetPile(Owner).Cards.Count;
        decimal damage = DynamicVars.Damage.BaseValue + exhaustedCards * DynamicVars["PerExhaust"].BaseValue;
        await DamageCmd.Attack(damage).FromCard(this).Targeting(cardPlay.Target).Execute(context);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
        DynamicVars["PerExhaust"].UpgradeValueBy(1m);
    }
}
