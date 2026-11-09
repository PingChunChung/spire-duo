namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DreamJournal() : KoalaCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int count = WasRetainedLastTurn ? 2 : 1;
        for (int i = 0; i < count; i++)
            await Dream.AddToHand(choiceContext, Owner, IsUpgraded);
    }

    private bool WasRetainedLastTurn { get; set; }
    public override Task AfterFlush(PlayerChoiceContext choiceContext, MegaCrit.Sts2.Core.Entities.Players.Player player, IReadOnlyCollection<CardModel> flushedCards, IReadOnlyCollection<CardModel> retainedCards)
    {
        if (player == Owner) WasRetainedLastTurn = retainedCards.Contains(this);
        return Task.CompletedTask;
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
