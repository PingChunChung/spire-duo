namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>One-turn marker gained from the first successful Munch each turn.</summary>
public sealed class FullPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public static async Task<bool> Consume(PlayerChoiceContext choiceContext, Creature creature, CardModel? source)
    {
        var full = creature.GetPower<FullPower>();
        if (full == null) return false;
        await PowerCmd.Remove(full);
        return true;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner)) await PowerCmd.Remove(this);
    }
}
