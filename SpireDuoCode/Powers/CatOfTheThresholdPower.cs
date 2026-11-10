using MegaCrit.Sts2.Core.Entities.Players;
using SpireDuo.SpireDuoCode.Cards;
using SpireDuo.SpireDuoCode.Progress;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class CatOfTheThresholdPower : SpireDuoPower
{
    private bool _used;
    private bool _pendingOmen;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override bool ShouldDieLate(Creature creature) => creature != Owner || _used;

    // Preventing death leaves the creature at 0 HP; like Lizard Tail, heal it back up ourselves.
    public override async Task AfterPreventingDeath(Creature creature)
    {
        MilestoneTracker.RecordThresholdPrevention(creature);
        Flash();
        _used = true;
        _pendingOmen = true;
        await CreatureCmd.Heal(creature, 1m);
    }

    public override async Task AfterDamageReceived(
        PlayerChoiceContext context,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? source)
    {
        // During the enemy turn the Omen's Energy and Retain would be wasted, so those wait for our next turn.
        if (_pendingOmen && target == Owner && Owner.CombatState?.CurrentSide == Owner.Side)
            await ResolveOmen(context);
    }

    // The killing blow itself skips AfterDamageReceived, so a death from our own card resolves once that card finishes.
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (_pendingOmen && cardPlay.Card.Owner == Owner.Player && Owner.CombatState?.CurrentSide == Owner.Side)
            await ResolveOmen(context);
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext context, Player player)
    {
        if (!_pendingOmen || player != Owner.Player)
            return;

        await ResolveOmen(context);
    }

    private async Task ResolveOmen(PlayerChoiceContext context)
    {
        if (!_pendingOmen)
            return;

        _pendingOmen = false;
        var play = new CardPlay
        {
            Card = Owner.CombatState!.CreateCard<BlackThread>(Owner.Player!),
            Target = null,
            ResultPile = PileType.Exhaust,
            Resources = default,
            IsAutoPlay = true,
            PlayIndex = 0,
            PlayCount = 1
        };
        await FootprintStatePower.Clear(context, Owner, play);
        await FootprintStatePower.SetAll(context, Owner, Footprint.Power, Footprint.Skill, Footprint.Power, play.Card);
        FootprintStatePower state = await FootprintStatePower.GetOrCreate(context, Owner, play.Card);
        await state.TriggerOmenNow(context, play);
        if (Amount > 1m)
            await CreatureCmd.GainBlock(Owner, Amount - 1m, ValueProp.Unpowered, play);
    }
}
