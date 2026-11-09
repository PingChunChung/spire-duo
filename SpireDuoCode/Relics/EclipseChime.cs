using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(EventRelicPool))]
public sealed class EclipseChime : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override async Task BeforeCombatStart()
    {
        Flash();
        var context = new ThrowingPlayerChoiceContext();
        await FootprintStatePower.Set(context, Owner.Creature, 0, Footprint.Attack);
        await FootprintStatePower.Set(context, Owner.Creature, 1, Footprint.Skill);
        await FootprintStatePower.Lock(context, Owner.Creature, 1);
    }
}
