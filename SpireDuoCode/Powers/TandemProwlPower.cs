namespace SpireDuo.SpireDuoCode.Powers;

public sealed class TandemProwlPower : FootprintListenerPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task OnCompanionResolved(PlayerChoiceContext context, CardPlay cardPlay, Creature? ally)
    {
        if (ally == null || ally.IsDead)
            return;
        Flash();
        await CreatureCmd.GainBlock(ally, Amount, ValueProp.Unpowered, cardPlay);
    }
}
