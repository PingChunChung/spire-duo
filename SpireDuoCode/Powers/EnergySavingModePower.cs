using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>The owner's unspent Energy carries over to the next turn (same rule as the Ice Cream relic).</summary>
public sealed class EnergySavingModePower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(this)];

    public override bool ShouldPlayerResetEnergy(Player player)
    {
        return player != Owner.Player || player.PlayerCombatState?.TurnNumber == 1;
    }
}
