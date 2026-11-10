using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;
[Pool(typeof(RaccoonRelicPool))]
public sealed class TrashCrown : SpireDuoRelic, IStashListener
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2m, ValueProp.Unpowered)];
    public async Task OnStashed(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source)
    {
        Flash();
        await CreatureCmd.Damage(context, Owner.Creature.CombatState!.HittableEnemies, DynamicVars.Damage.BaseValue, ValueProp.Unpowered, Owner.Creature, source);
    }
}
