using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class UnwantedGift() : RaccoonCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        if (p.Target != null && await RaccoonStashStatePower.Spend(c, Owner.Creature, this, x => x.Type == CardType.Attack) != null)
            await PowerCmd.Apply<WeakPower>(c, p.Target, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade() => DynamicVars["WeakPower"].UpgradeValueBy(1m);
}
