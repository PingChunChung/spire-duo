using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Events;

namespace SpireDuo.SpireDuoCode.Events;

/// <summary>Act 2 co-op vote. Delayed combat effects are replaced with immediate individual rewards.</summary>
public sealed class BorrowedHammock : SpireDuoEvent
{
    public override bool IsShared => true;
    protected override int[] ActIndices => [1];
    protected override bool RequiresMultiplayer => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new MaxHpVar(5m), new GoldVar(50), new DynamicVar("BothMaxHp", 8m)];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() => [Option(TakeTurns)];

    private Task TakeTurns()
    {
        SetEventState(PageDescription("SCHEDULE"), [Option(KoalaFirst, "SCHEDULE"), Option(PandaFirst, "SCHEDULE"), Option(BothClimbIn, "SCHEDULE")]);
        return Task.CompletedTask;
    }

    private async Task KoalaFirst()
    {
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
        var card = (await CardSelectCmd.FromDeckForUpgrade(Owner, new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, 1))).FirstOrDefault();
        if (card != null) CardCmd.Upgrade(card);
        SetEventFinished(PageDescription("KOALA_FIRST"));
    }

    private async Task PandaFirst()
    {
        var cards = await CardSelectCmd.FromDeckForRemoval(
            prefs: new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 1),
            player: Owner);
        await CardPileCmd.RemoveFromDeck(cards.ToList());
        await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner);
        SetEventFinished(PageDescription("PANDA_FIRST"));
    }

    private async Task BothClimbIn()
    {
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars["BothMaxHp"].BaseValue);
        SetEventFinished(PageDescription("BOTH_CLIMB_IN"));
    }
}
