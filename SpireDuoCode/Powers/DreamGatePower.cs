using SpireDuo.SpireDuoCode.Cards;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class DreamGatePower : SpireDuoPower
{
    private int _dreamsPlayedThisTurn;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount) =>
        card.Owner == Owner.Player && card is Dream && _dreamsPlayedThisTurn < Amount ? playCount + 1 : playCount;

    public override Task AfterModifyingCardPlayCount(CardModel card)
    {
        if (card.Owner == Owner.Player && card is Dream && _dreamsPlayedThisTurn < Amount)
        {
            _dreamsPlayedThisTurn++;
            Flash();
        }
        return Task.CompletedTask;
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, MegaCrit.Sts2.Core.Entities.Players.Player player)
    {
        if (player == Owner.Player) _dreamsPlayedThisTurn = 0;
        return Task.CompletedTask;
    }
}
