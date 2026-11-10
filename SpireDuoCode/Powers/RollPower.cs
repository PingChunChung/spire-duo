using SpireDuo.SpireDuoCode.Progress;

namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Momentum: each Attack played adds 1 Roll after it resolves. Roll payoffs spend all of it.</summary>
public sealed class RollPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public static Task Gain(PlayerChoiceContext choiceContext, Creature creature, decimal amount, CardModel? source) =>
        PowerCmd.Apply<RollPower>(choiceContext, creature, amount, creature, source);

    public static async Task<int> SpendAll(Creature creature)
    {
        var roll = creature.GetPower<RollPower>();
        if (roll == null) return 0;
        int amount = roll.Amount;
        await PowerCmd.Remove(roll);
        return amount;
    }

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this && amount > 0)
            MilestoneTracker.RecordRoll(Owner, Amount);
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card.Type != CardType.Attack) return;
        Flash();
        await Gain(choiceContext, Owner, 1m, cardPlay.Card);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner)) await PowerCmd.Remove(this);
    }
}
