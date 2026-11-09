namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Convenience base for powers which listen to the hidden Footprint state.</summary>
public abstract class FootprintListenerPower : SpireDuoPower, IFootprintListener
{
    public virtual Task OnFootprintLine(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line) => Task.CompletedTask;
    public virtual Task OnFootprintSetOrRotated(PlayerChoiceContext context, CardModel? source) => Task.CompletedTask;
    public virtual Task OnCompanionResolved(PlayerChoiceContext context, CardPlay cardPlay, Creature? ally) => Task.CompletedTask;
    public virtual Task OnEmptySlotFilled(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
    public virtual Task OnFootprintCleared(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
}
