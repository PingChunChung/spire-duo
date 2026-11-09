using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(BlackCatRelicPool))]
public sealed class MoonbellCollar : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override async Task BeforeCombatStart()
    {
        Flash();
        await FootprintStatePower.Set(new ThrowingPlayerChoiceContext(), Owner.Creature, 1, Footprint.Skill);
    }
}
