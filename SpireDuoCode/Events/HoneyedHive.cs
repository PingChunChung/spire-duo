using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Rewards;

namespace SpireDuo.SpireDuoCode.Events;

/// <summary>Act 1 co-op vote. Combat uses a vanilla Overgrowth normal encounter.</summary>
public sealed class HoneyedHive : SpireDuoEvent
{
    public override bool IsShared => true;
    protected override int[] ActIndices => [0];
    protected override bool RequiresMultiplayer => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new GoldVar(75), new HealVar(7m)];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() => [Option(TakeTheHive), Option(BackAway)];

    private async Task TakeTheHive()
    {
        await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner);
        EnterCombatWithoutExitingEvent<FlyconidNormal>(Array.Empty<Reward>(), shouldResumeAfterCombat: false);
    }

    private async Task BackAway()
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        SetEventFinished(PageDescription("BACK_AWAY"));
    }
}
