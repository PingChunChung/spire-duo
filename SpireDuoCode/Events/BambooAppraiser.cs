using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using SpireDuo.SpireDuoCode.Enchantments;

namespace SpireDuo.SpireDuoCode.Events;

/// <summary>Act 2 individual event. Delayed resource is intentionally replaced by an immediate upgrade.</summary>
public sealed class BambooAppraiser : SpireDuoEvent
{
    protected override int[] ActIndices => [1];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new GoldVar(50), new CardsVar(1)];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() =>
    [
        Owner.Relics.Any(relic => relic.IsTradable) ? Option(TradeARelic) : LockedOption("TRADE_A_RELIC_LOCKED"),
        Owner.Gold >= DynamicVars.Gold.IntValue ? Option(AppraiseIt) : LockedOption("APPRAISE_IT_LOCKED"),
        Option(Leave),
    ];

    private async Task TradeARelic()
    {
        var relics = Owner.Relics.Where(relic => relic.IsTradable).ToList();
        var relic = await RelicSelectCmd.FromChooseARelicScreen(Owner, relics);
        if (relic != null)
        {
            await RelicCmd.Remove(relic);
            await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(Owner).ToMutable(), Owner);
        }
        SetEventFinished(PageDescription("TRADE_A_RELIC"));
    }

    private async Task AppraiseIt()
    {
        await PlayerCmd.LoseGold(DynamicVars.Gold.IntValue, Owner, GoldLossType.Spent);
        var card = (await CardSelectCmd.FromDeckForUpgrade(Owner,
            new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, DynamicVars.Cards.IntValue))).FirstOrDefault();
        if (card != null) CardCmd.Upgrade(card);
        SetEventState(PageDescription("APPRAISE_IT"),
        [
            HasMunchCard() ? Option(FreshCut, "APPRAISE_IT") : LockedOption("FRESH_CUT_LOCKED", "APPRAISE_IT"),
            Option(Leave, "APPRAISE_IT"),
        ]);
    }

    private async Task FreshCut()
    {
        var cards = await CardSelectCmd.FromDeckForEnchantment(Owner, ModelDb.Enchantment<FreshlyCut>(), 1,
            new CardSelectorPrefs(L10NLookup($"{Id.Entry}.pages.APPRAISE_IT.selectionScreenPrompt"), 1));
        var card = cards.FirstOrDefault();
        if (card != null) CardCmd.Enchant(ModelDb.Enchantment<FreshlyCut>().ToMutable(), card, 1m);
        SetEventFinished(PageDescription("FRESH_CUT"));
    }

    private Task Leave()
    {
        SetEventFinished(PageDescription("LEAVE"));
        return Task.CompletedTask;
    }

    private bool HasMunchCard() => Owner.Deck.Cards.Any(card =>
        ModelDb.Enchantment<FreshlyCut>().CanEnchant(card));
}
