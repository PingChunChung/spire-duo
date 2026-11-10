using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Rewards;
using SpireDuo.SpireDuoCode.Cards;
using SpireDuo.SpireDuoCode.Progress;

namespace SpireDuo.SpireDuoCode.Events;

/// <summary>Act 2 co-op vote. Each event instance resumes and rewards only its owner.</summary>
public sealed class RainyDayArgument : SpireDuoEvent
{
    public override bool IsShared => true;
    protected override int[] ActIndices => [1];
    protected override bool RequiresMultiplayer => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(12m), new HpLossVar(10m), new GoldVar(100)];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() => [Option(ProveIt), Option(MakeTea), Option(WalkAway)];

    private Task ProveIt()
    {
        EnterCombatWithoutExitingEvent<BowlbugsNormal>(Array.Empty<Reward>(), shouldResumeAfterCombat: true);
        return Task.CompletedTask;
    }

    public override async Task Resume(AbstractRoom room)
    {
        var card = Owner.RunState.CreateCard<MakeUp>(Owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck));
        SetEventFinished(PageDescription("AFTER_FIGHT"));
    }

    private async Task MakeTea()
    {
        MilestoneTracker.Unlock(MilestoneIds.MakeUp);
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        SetEventFinished(PageDescription("MAKE_TEA"));
    }

    private async Task WalkAway()
    {
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
        await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner);
        SetEventFinished(PageDescription("WALK_AWAY"));
    }
}
