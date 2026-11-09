using MegaCrit.Sts2.Core.Extensions;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class MidnightOraclePower : SpireDuoPower
{
    private int _turn = -1;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1;
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card.Type != CardType.Power || turn == _turn)
            return;
        _turn = turn;
        var cards = PileType.Hand.GetPile(Owner.Player!).Cards.Where(card => !card.EnergyCost.CostsX).ToList();
        foreach (var card in cards.StableShuffle(Owner.Player.RunState.Rng.CombatCardSelection).Take(Amount))
            card.EnergyCost.SetThisTurn(0);
        Flash();
        await Task.CompletedTask;
    }
}
