using MegaCrit.Sts2.Core.Entities.Players;
using SpireDuo.SpireDuoCode.Cards;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class CatOfTheThresholdPower : SpireDuoPower
{
    private bool _used;
    private bool _pendingOmen;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override bool ShouldDieLate(Creature creature) => creature != Owner || _used;

    public override Task AfterPreventingDeath(Creature creature)
    {
        _used = true;
        _pendingOmen = true;
        return Task.CompletedTask;
    }

    public override async Task AfterDamageReceived(
        PlayerChoiceContext context,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? source)
    {
        if (_pendingOmen && target == Owner)
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
            Card = ModelDb.Card<BlackThread>(),
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
