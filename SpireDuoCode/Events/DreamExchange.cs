using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models.Cards;

namespace SpireDuo.SpireDuoCode.Events;

/// <summary>Act 3 individual, two pages. The deposit is a safe Retain-card upgrade.</summary>
public sealed class DreamExchange : SpireDuoEvent
{
    protected override int[] ActIndices => [2];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(20m), new CardsVar(1)];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() => [Option(DepositADream), Option(WithdrawOne), Option(CloseAccount)];

    private Task DepositADream()
    {
        SetEventState(PageDescription("DEPOSIT"),
        [
            HasUpgradableRetainCard() ? Option(UpgradeRetain, "DEPOSIT") : LockedOption("UPGRADE_RETAIN_LOCKED", "DEPOSIT"),
            Option(CloseAccount, "DEPOSIT"),
        ]);
        return Task.CompletedTask;
    }

    private async Task UpgradeRetain()
    {
        var prefs = new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, DynamicVars.Cards.IntValue);
        var card = (await CardSelectCmd.FromDeckGeneric(Owner, prefs, c => c.Keywords.Contains(CardKeyword.Retain) && c.IsUpgradable)).FirstOrDefault();
        if (card != null) CardCmd.Upgrade(card);
        SetEventFinished(PageDescription("UPGRADE_RETAIN"));
    }

    private async Task WithdrawOne()
    {
        decimal loss = Owner.Creature.CurrentHp * DynamicVars.HpLoss.BaseValue / 100m;
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner.Creature, loss, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
        var cards = Owner.Character.CardPool
            .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
            .Where(card => card.Rarity == CardRarity.Rare)
            .ToList();
        var card = Owner.RunState.CreateCard(Owner.PlayerRng.Rewards.NextItem(cards)!, Owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck));
        SetEventFinished(PageDescription("WITHDRAW_ONE"));
    }

    private Task CloseAccount()
    {
        SetEventFinished(PageDescription("CLOSE_ACCOUNT"));
        return Task.CompletedTask;
    }

    private bool HasUpgradableRetainCard() => Owner.Deck.Cards.Any(card =>
        card.Keywords.Contains(CardKeyword.Retain) && card.IsUpgradable);
}
