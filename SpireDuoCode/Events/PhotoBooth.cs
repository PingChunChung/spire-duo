using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;

namespace SpireDuo.SpireDuoCode.Events;

/// <summary>Any act.</summary>
public sealed class PhotoBooth : SpireDuoEvent
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new GoldVar(50), new CardsVar(2)];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() =>
    [
        Owner.Gold >= DynamicVars.Gold.IntValue ? Option(Photo) : LockedOption("PHOTO_LOCKED"),
        Option(SillyFaces),
    ];

    private async Task Photo()
    {
        await PlayerCmd.LoseGold(DynamicVars.Gold.IntValue, Owner, GoldLossType.Spent);
        var prefs = new CardSelectorPrefs(L10NLookup($"{Id.Entry}.pages.PHOTO.selectionScreenPrompt"), 1);
        var card = (await CardSelectCmd.FromDeckGeneric(Owner, prefs, c => c.Type != CardType.Quest)).FirstOrDefault();
        if (card != null)
        {
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CloneCard(card), PileType.Deck));
        }
        SetEventFinished(PageDescription("PHOTO"));
    }

    private Task SillyFaces()
    {
        var cards = PileType.Deck.GetPile(Owner).Cards.Where(c => c?.IsUpgradable ?? false).ToList().StableShuffle(Rng)
            .Take(DynamicVars.Cards.IntValue);
        foreach (var card in cards)
        {
            CardCmd.Upgrade(card);
        }
        SetEventFinished(PageDescription("SILLY_FACES"));
        return Task.CompletedTask;
    }
}
