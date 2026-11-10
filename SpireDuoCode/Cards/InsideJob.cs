using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class InsideJob() : RaccoonCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        if (await RaccoonStashStatePower.Spend(c, Owner.Creature, this) == null)
            return;
        await PowerCmd.Apply<InsideJobPower>(c, Owner.Creature, 1m, Owner.Creature, this);
        if (IsUpgraded)
            await CardPileCmd.Draw(c, 1, Owner);
    }
    protected override void OnUpgrade()
    {
    }
}
