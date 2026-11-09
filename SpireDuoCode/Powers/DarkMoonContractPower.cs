using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class DarkMoonContractPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext context, Player player)
    {
        if (player != Owner.Player)
            return;

        await FootprintStatePower.Set(context, Owner, 0, Footprint.Power);
        await CardPileCmd.Draw(context, 1, player);
        if (Amount > 1m)
            await CreatureCmd.GainBlock(Owner, Amount - 1m, ValueProp.Unpowered, null);
    }
}
