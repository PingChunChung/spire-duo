namespace SpireDuo.SpireDuoCode.Powers;

public sealed class AlphaPandaReadyPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource) =>
        dealer == Owner && props.IsPoweredAttack() ? amount : 0m;
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card.Type != CardType.Attack) return;
        await PowerCmd.ModifyAmount(choiceContext, this, -1m, Owner, cardPlay.Card);
        if (Amount <= 1m) await PowerCmd.Remove(this);
    }
}
