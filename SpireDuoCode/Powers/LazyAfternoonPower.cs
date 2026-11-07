using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever the owner Retains cards at the end of the turn, gain that much Drowsy.</summary>
public sealed class LazyAfternoonPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain), HoverTipFactory.FromPower<DrowsyPower>()];

    public override async Task AfterFlush(PlayerChoiceContext choiceContext, Player player, IReadOnlyCollection<CardModel> flushedCards, IReadOnlyCollection<CardModel> retainedCards)
    {
        if (player != Owner.Player || retainedCards.Count == 0) return;
        Flash();
        await PowerCmd.Apply<DrowsyPower>(choiceContext, Owner, retainedCards.Count * Amount, Owner, null);
    }
}
