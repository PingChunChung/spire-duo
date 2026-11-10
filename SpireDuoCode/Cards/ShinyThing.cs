using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class ShinyThing() : RaccoonCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        bool high = PileType.Draw.GetPile(Owner).Cards.FirstOrDefault() is
        {
        }
        card && RaccoonStashStatePower.CostValue(card) >= 2;
        if (await RaccoonStashStatePower.StashTopDraw(c, Owner, this)
            && high)
        {
            if (IsUpgraded)
                await PlayerCmd.GainEnergy(1, Owner);
            else await CardPileCmd.Draw(c, 1, Owner);
        }
    }
    protected override void OnUpgrade()
    {
    }
}
