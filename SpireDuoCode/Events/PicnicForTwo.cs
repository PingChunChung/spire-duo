using MegaCrit.Sts2.Core.Events;
using SpireDuo.SpireDuoCode.Progress;

namespace SpireDuo.SpireDuoCode.Events;

/// <summary>Act 1. Shared, so the players vote once and the winning option applies to each of them.</summary>
public sealed class PicnicForTwo : SpireDuoEvent
{
    public override bool IsShared => true;
    protected override int[] ActIndices => [0];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(10m), new MaxHpVar(5m)];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() => [Option(Share), Option(Feast)];

    private async Task Share()
    {
        MilestoneTracker.Unlock(MilestoneIds.Picnic);
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        SetEventFinished(PageDescription("SHARE"));
    }

    private async Task Feast()
    {
        MilestoneTracker.Unlock(MilestoneIds.Picnic);
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
        SetEventFinished(PageDescription("FEAST"));
    }
}
