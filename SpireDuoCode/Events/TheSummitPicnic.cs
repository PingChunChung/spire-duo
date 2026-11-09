using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Rewards;
using SpireDuo.SpireDuoCode.Potions;

namespace SpireDuo.SpireDuoCode.Events;

/// <summary>Act 3 co-op vote. All effects are scoped to the receiving event instance owner.</summary>
public sealed class TheSummitPicnic : SpireDuoEvent
{
    public override bool IsShared => true;
    protected override int[] ActIndices => [2];
    protected override bool RequiresMultiplayer => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new MaxHpVar(10m), new GoldVar(150), new HealVar(20m)];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() => [Option(SetTheTable), Option(ShareRations)];

    private Task SetTheTable()
    {
        SetEventState(PageDescription("MENU"), [Option(Feast, "MENU"), Option(PackLight, "MENU")]);
        return Task.CompletedTask;
    }

    private async Task Feast()
    {
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
        await RewardsCmd.OfferCustom(Owner, [new PotionReward(ModelDb.Potion<HalfAndHalf>().ToMutable(), Owner)]);
        SetEventFinished(PageDescription("FEAST"));
    }

    private async Task PackLight()
    {
        var cards = await CardSelectCmd.FromDeckForRemoval(
            prefs: new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 1), player: Owner);
        await CardPileCmd.RemoveFromDeck(cards.ToList());
        await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner);
        SetEventFinished(PageDescription("PACK_LIGHT"));
    }

    private async Task ShareRations()
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        SetEventFinished(PageDescription("SHARE_RATIONS"));
    }
}
