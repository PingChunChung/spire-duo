using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class WashedClean() : RaccoonCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        if (await RaccoonStashStatePower.Spend(c, Owner.Creature, this) != null) foreach (var power in Owner.Creature.Powers.Where(x => x.Type == PowerType.Debuff).ToArray())
            await PowerCmd.Remove(power);
        if (IsUpgraded)
            await CardPileCmd.Draw(c, 1, Owner);
    }
    protected override void OnUpgrade()
    {
    }
}
