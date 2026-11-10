using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class LooseChange() : RaccoonCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [EnergyHoverTip];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        if (RaccoonStashStatePower.Has(Owner.Creature, x => RaccoonStashStatePower.CostValue(x) >= 2))
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        if (IsUpgraded)
            await CardPileCmd.Draw(c, 1, Owner);
    }
    protected override void OnUpgrade()
    {
    }
}
