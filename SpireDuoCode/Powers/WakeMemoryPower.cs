namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Internal, one-turn marker used by cards that care whether their owner woke this turn.</summary>
public sealed class WakeMemoryPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner)) await PowerCmd.Remove(this);
    }
}
