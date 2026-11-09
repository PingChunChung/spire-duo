using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>At the start of the owner's next turn, restores a capped amount of their prior unspent Energy.</summary>
public sealed class EnergySavingModePower : SpireDuoPower
{
    private class Data
    {
        public int savedEnergy;
    }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(this)];

    protected override object InitInternalData() => new Data();

    public override Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner))
            GetInternalData<Data>().savedEnergy = (int)Math.Min(Amount, Owner.Player?.PlayerCombatState?.Energy ?? 0);
        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;
        var data = GetInternalData<Data>();
        if (data.savedEnergy <= 0) return;
        Flash();
        await PlayerCmd.GainEnergy(data.savedEnergy, player);
        data.savedEnergy = 0;
    }
}
