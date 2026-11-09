using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models.Cards;

namespace SpireDuo.SpireDuoCode.Events;

/// <summary>Act 1 co-op vote. Each event instance rewards only its own owner.</summary>
public sealed class TheLastDumpling : SpireDuoEvent
{
    public override bool IsShared => true;
    protected override int[] ActIndices => [0];
    protected override bool RequiresMultiplayer => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(12m), new MaxHpVar(8m), new CardsVar(1)];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() => [Option(Split), Option(ShareTheChoice), Option(LeaveIt)];

    private async Task Split()
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        SetEventFinished(PageDescription("SPLIT"));
    }

    private async Task ShareTheChoice()
    {
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
        await CardPileCmd.Add(Owner.RunState.CreateCard<Wound>(Owner), PileType.Deck);
        SetEventFinished(PageDescription("SHARE_THE_CHOICE"));
    }

    private Task LeaveIt()
    {
        var card = PileType.Deck.GetPile(Owner).Cards.Where(c => c.IsUpgradable).ToList().StableShuffle(Rng).FirstOrDefault();
        if (card != null) CardCmd.Upgrade(card);
        SetEventFinished(PageDescription("LEAVE_IT"));
        return Task.CompletedTask;
    }
}
