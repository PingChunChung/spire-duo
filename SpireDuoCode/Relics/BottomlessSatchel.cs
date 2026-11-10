using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace SpireDuo.SpireDuoCode.Relics;
[Pool(typeof(EventRelicPool))]
public sealed class BottomlessSatchel : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override Task BeforeCombatStart()
    {
        return StashTopTwo();
    }
    private async Task StashTopTwo()
    {
        var context = new ThrowingPlayerChoiceContext();
        await RaccoonStashStatePower.StashTopDraw(context, Owner.Creature.Player!, null!);
        await RaccoonStashStatePower.StashTopDraw(context, Owner.Creature.Player!, null!);
    }
}
