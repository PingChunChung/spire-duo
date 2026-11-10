using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class HiddenMarketPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext c, Player p)
    {
        if (p != Owner.Player || Owner.GetPower<RaccoonStashStatePower>()?.Count == 0)
            return;
        var card = await RaccoonStashStatePower.Unstash(c, Owner, null!);
        if (card != null)
            card.EnergyCost.SetThisTurn(0);
    }
}
