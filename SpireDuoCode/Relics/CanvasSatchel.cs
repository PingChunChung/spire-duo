using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;
[Pool(typeof(RaccoonRelicPool))]
public sealed class CanvasSatchel : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override Task BeforeCombatStart()
    {
        return RaccoonStashStatePower.StashTopDraw(new ThrowingPlayerChoiceContext(), Owner.Creature.Player!, null!);
    }
}
