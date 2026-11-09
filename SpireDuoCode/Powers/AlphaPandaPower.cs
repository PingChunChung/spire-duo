namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Turns each Strength gain into a temporary number of doubled Attack plays.</summary>
public sealed class AlphaPandaPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power.Owner != Owner || power is not StrengthPower || amount <= 0) return;
        Flash();
        await PowerCmd.Apply<AlphaPandaReadyPower>(choiceContext, Owner, Amount, Owner, cardSource);
    }
}
