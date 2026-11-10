using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class InsideJobPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card.Type != CardType.Attack)
            return;
        await PowerCmd.Remove(this);
        Flash();
        var copy = cardPlay.Card.CreateClone();
        copy.EnergyCost.SetThisTurn(0);
        await CardPileCmd.AddGeneratedCardToCombat(copy, PileType.Hand, Owner.Player!);
        await CardCmd.AutoPlay(context, copy, cardPlay.Target, AutoPlayType.Default);
    }
    public override Task BeforeSideTurnEnd(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature > participants) => participants.Contains(Owner)? PowerCmd.Remove(this) : Task.CompletedTask;
}
