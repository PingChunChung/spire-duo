using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(PandaRelicPool))]
public sealed class SplitBamboo : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(8m, ValueProp.Unpowered),
        new DynamicVar("Threshold", 8m),
        new PowerVar<BambooPower>(4m),
        new DamageVar(16m, ValueProp.Unpowered)
    ];

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner.Creature)) return;

        int bamboo = Owner.Creature.GetPowerAmount<BambooPower>();
        if (bamboo == 0)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.BaseValue, ValueProp.Unpowered, null);
            return;
        }

        if (bamboo < DynamicVars["Threshold"].IntValue) return;

        await PowerCmd.ModifyAmount(choiceContext, Owner.Creature.GetPower<BambooPower>()!, -DynamicVars["BambooPower"].BaseValue, Owner.Creature, null);
        var enemies = Owner.Creature.CombatState!.HittableEnemies;
        if (enemies.Count == 0) return;

        var target = Owner.RunState.Rng.CombatTargets.NextItem(enemies)!;
        await CreatureCmd.Damage(choiceContext, target, DynamicVars.Damage.BaseValue, ValueProp.Unpowered, Owner.Creature, null);
    }
}
