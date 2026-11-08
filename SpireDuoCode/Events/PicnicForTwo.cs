using MegaCrit.Sts2.Core.Events;

namespace SpireDuo.SpireDuoCode.Events;

/// <summary>Act 1. Sharing heals every player, so a couple who both share heal twice.</summary>
public sealed class PicnicForTwo : SpireDuoEvent
{
    protected override int[] ActIndices => [0];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(10m), new MaxHpVar(5m)];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() => [Option(Share), Option(Feast)];

    private async Task Share()
    {
        foreach (var player in Owner.RunState.Players.Where(p => p.Creature.IsAlive))
        {
            await CreatureCmd.Heal(player.Creature, DynamicVars.Heal.BaseValue);
        }
        SetEventFinished(PageDescription("SHARE"));
    }

    private async Task Feast()
    {
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
        SetEventFinished(PageDescription("FEAST"));
    }
}
