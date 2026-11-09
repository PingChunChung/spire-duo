using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class DeepScriptPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain), HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task AfterFlush(PlayerChoiceContext choiceContext, Player player, IReadOnlyCollection<CardModel> flushedCards, IReadOnlyCollection<CardModel> retainedCards)
    {
        if (player != Owner.Player || retainedCards.Count == 0) return;
        Flash();
        foreach (var card in retainedCards)
        {
            if (card.DynamicVars.TryGetValue("Damage", out var damage)) damage.BaseValue += Amount;
            if (card.DynamicVars.TryGetValue("Block", out var block)) block.BaseValue += Amount;
            await CreatureCmd.GainBlock(Owner, 1, ValueProp.Unpowered, null);
        }
    }
}
