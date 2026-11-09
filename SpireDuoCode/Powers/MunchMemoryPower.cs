namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Internal one-turn marker for effects that care whether the owner Munched this turn.</summary>
public sealed class MunchMemoryPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner)) await PowerCmd.Remove(this);
    }
}
