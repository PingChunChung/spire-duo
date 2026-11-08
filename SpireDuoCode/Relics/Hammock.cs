using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(KoalaRelicPool))]
public sealed class Hammock : SpireDuoRelic
{
    private bool _retainedLastTurn;

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain)];

    public override Task BeforeCombatStart()
    {
        _retainedLastTurn = false;
        return Task.CompletedTask;
    }

    public override Task AfterFlush(PlayerChoiceContext choiceContext, Player player, IReadOnlyCollection<CardModel> flushedCards, IReadOnlyCollection<CardModel> retainedCards)
    {
        if (player == Owner) _retainedLastTurn = retainedCards.Count > 0;
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature) || !_retainedLastTurn) return;
        _retainedLastTurn = false;
        Flash();
        await CardPileCmd.Draw(new ThrowingPlayerChoiceContext(), DynamicVars.Cards.BaseValue, Owner);
    }
}
