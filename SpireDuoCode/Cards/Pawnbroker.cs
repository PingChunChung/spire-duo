using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Pawnbroker() : RaccoonCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        var card = await RaccoonStashStatePower.Spend(c, Owner.Creature, this);
        if (card != null) await CreatureCmd.GainBlock(Owner.Creature,(IsUpgraded? 4m : 1m)
            * RaccoonStashStatePower.CostValue(card), ValueProp.Move, p);
        if (card != null)
            await CardPileCmd.Draw(c, 1, Owner);
    }
    protected override void OnUpgrade()
    {
    }
}
