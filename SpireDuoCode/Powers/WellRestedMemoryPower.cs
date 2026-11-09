namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Tracks Well-Rested triggers for one turn and caps their total Block at five.</summary>
public sealed class WellRestedMemoryPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public static async Task TryGain(PlayerChoiceContext choiceContext, Creature creature, CardModel source)
    {
        var memory = creature.GetPower<WellRestedMemoryPower>();
        if (memory != null && memory.Amount >= 5m) return;
        await CreatureCmd.GainBlock(creature, new BlockVar(1m, ValueProp.Unpowered), null);
        if (memory == null)
            await PowerCmd.Apply<WellRestedMemoryPower>(choiceContext, creature, 1m, creature, source);
        else
            await PowerCmd.ModifyAmount(choiceContext, memory, 1m, creature, source);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner)) await PowerCmd.Remove(this);
    }
}
