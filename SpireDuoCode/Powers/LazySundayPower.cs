using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Every card the owner Retains costs 0 until it is played.</summary>
public sealed class LazySundayPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain)];

    public override Task AfterFlush(PlayerChoiceContext choiceContext, Player player, IReadOnlyCollection<CardModel> flushedCards, IReadOnlyCollection<CardModel> retainedCards)
    {
        if (player != Owner.Player || retainedCards.Count == 0) return Task.CompletedTask;
        Flash();
        foreach (var card in retainedCards)
        {
            card.EnergyCost.SetUntilPlayed(0, reduceOnly: true);
        }
        return Task.CompletedTask;
    }
}
