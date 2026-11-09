using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using SpireDuo.SpireDuoCode.Enchantments;

namespace SpireDuo.SpireDuoCode.Events;

/// <summary>Act 1 individual event. A short delay can improve one retained card.</summary>
public sealed class EucalyptusTollbooth : SpireDuoEvent
{
    protected override int[] ActIndices => [0];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new GoldVar(35), new CardsVar(2)];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() =>
    [
        HasRetainCard() ? Option(NapInLine) : LockedOption("NAP_IN_LINE_LOCKED"),
        Owner.Gold >= DynamicVars.Gold.IntValue ? Option(Pay) : LockedOption("PAY_LOCKED"),
        Option(TurnAround),
    ];

    private async Task NapInLine()
    {
        var enchantment = ModelDb.Enchantment<WellRested>();
        var cards = await CardSelectCmd.FromDeckForEnchantment(Owner, enchantment, 1,
            new CardSelectorPrefs(L10NLookup($"{Id.Entry}.pages.INITIAL.selectionScreenPrompt"), 1));
        var card = cards.FirstOrDefault();
        if (card != null) CardCmd.Enchant(enchantment.ToMutable(), card, 1m);
        SetEventFinished(PageDescription("NAP_IN_LINE"));
    }

    private async Task Pay()
    {
        await PlayerCmd.LoseGold(DynamicVars.Gold.IntValue, Owner, GoldLossType.Spent);
        var cards = Owner.Deck.Cards
            .Where(card => card.IsUpgradable)
            .ToList()
            .StableShuffle(Rng)
            .Take(DynamicVars.Cards.IntValue);
        foreach (var card in cards) CardCmd.Upgrade(card);
        SetEventFinished(PageDescription("PAY"));
    }

    private bool HasRetainCard() => Owner.Deck.Cards.Any(card =>
        ModelDb.Enchantment<WellRested>().CanEnchant(card));

    private Task TurnAround()
    {
        SetEventFinished(PageDescription("TURN_AROUND"));
        return Task.CompletedTask;
    }
}
