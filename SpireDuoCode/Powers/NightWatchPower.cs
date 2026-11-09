using MegaCrit.Sts2.Core.Entities.Players;
using SpireDuo.SpireDuoCode.Cards;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class NightWatchPower : SpireDuoPower
{
    private bool _gainedDrowsy;
    private bool _grantDream;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power is DrowsyPower && power.Owner == Owner && amount > 0) _gainedDrowsy = true;
        await Task.CompletedTask;
    }

    public override Task AfterFlush(PlayerChoiceContext choiceContext, Player player, IReadOnlyCollection<CardModel> flushedCards, IReadOnlyCollection<CardModel> retainedCards)
    {
        if (player == Owner.Player) _grantDream = !_gainedDrowsy;
        _gainedDrowsy = false;
        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player || !_grantDream) return;
        _grantDream = false;
        Flash();
        await Dream.AddToHand(choiceContext, player, Amount >= 2);
        if (Amount >= 2) await CreatureCmd.GainBlock(Owner, 3, ValueProp.Unpowered, null);
    }
}
