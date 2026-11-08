using MegaCrit.Sts2.Core.Entities.Players;
using SpireDuo.SpireDuoCode.Extensions;

namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>At the start of the owner's next turn, gain this much Bamboo, then remove this power. Reuses the Bamboo icon.</summary>
public sealed class BambooNextTurnPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "bamboo_power.png".PowerImagePath();
    public override string CustomBigIconPath => "bamboo_power.png".BigPowerImagePath();

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;
        await PowerCmd.Apply<BambooPower>(choiceContext, Owner, Amount, Owner, null);
        await PowerCmd.Remove(this);
    }
}
