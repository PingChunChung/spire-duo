namespace SpireDuo.SpireDuoCode.Powers;

public sealed class BlackThreadPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner))
            return;
        var state = Owner.GetPower<FootprintStatePower>();
        if (state == null || state.Slots.Count(slot => slot == Footprint.Empty) < Amount)
            return;
        Flash();
        await CardPileCmd.Draw(context, 1, Owner.Player!);
    }
}
