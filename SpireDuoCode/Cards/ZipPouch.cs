using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class ZipPouch() : RaccoonCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await RaccoonCardEffects.Stash(c, this, 1, card => IsUpgraded ||(!card.EnergyCost.CostsX && card.EnergyCost.GetWithModifiers(CostModifiers.Local) == 0));
        await CardPileCmd.Draw(c, 1, Owner);
    }
    protected override void OnUpgrade()
    {
    }
}
