using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class KingOfTheAlleyPower : SpireDuoPower
{
    private bool _used;
    private bool _pendingFreeSpend;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override bool ShouldDieLate(Creature creature) => creature != Owner || _used;
    public override async Task AfterPreventingDeath(Creature creature)
    {
        _used = true;
        _pendingFreeSpend = true;
        Flash();
        await CreatureCmd.Heal(creature, 1m);
    }
    public override Task AfterDamageReceived(PlayerChoiceContext context, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? source) => _pendingFreeSpend && target == Owner && Owner.CombatState?.CurrentSide == Owner.Side? Resolve(context) : Task.CompletedTask;
    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay) => _pendingFreeSpend && cardPlay.Card.Owner == Owner.Player && Owner.CombatState?.CurrentSide == Owner.Side? Resolve(context) : Task.CompletedTask;
    public override Task AfterPlayerTurnStart(PlayerChoiceContext context, Player player) => _pendingFreeSpend && player == Owner.Player? Resolve(context) : Task.CompletedTask;
    private async Task Resolve(PlayerChoiceContext context)
    {
        if (!_pendingFreeSpend)
            return;
        _pendingFreeSpend = false;
        await RaccoonStashStatePower.StashRandomDiscard(context, Owner.Player!, 3, null!);
        await RaccoonStashStatePower.SpendUpTo(context, Owner, null!, 1);
    }
}
