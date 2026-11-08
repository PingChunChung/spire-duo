using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>At the start of the owner's turn, if they have no Drowsy, they gain Drowsy and draw a card.</summary>
public sealed class NightLightPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player || Owner.GetPowerAmount<DrowsyPower>() > 0) return;
        Flash();
        await PowerCmd.Apply<DrowsyPower>(choiceContext, Owner, Amount, Owner, null);
        await CardPileCmd.Draw(choiceContext, 1, player);
    }
}
