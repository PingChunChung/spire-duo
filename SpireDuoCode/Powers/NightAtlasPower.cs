using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class NightAtlasPower : FootprintListenerPower
{
    private int _turn = -1;
    private FootprintLine? _firstLine;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task OnFootprintLine(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1;
        if (turn != _turn)
        {
            _turn = turn;
            _firstLine = line;
            await PlayerCmd.GainEnergy(1, Owner.Player!);
            await CardPileCmd.Draw(context, 1, Owner.Player!);
            return;
        }

        if (_firstLine != line)
        {
            _firstLine = null;
            FootprintStatePower state = await FootprintStatePower.GetOrCreate(context, Owner, cardPlay.Card);
            await state.TriggerLineNow(context, cardPlay, line);
            if (Amount > 1)
                await CreatureCmd.GainBlock(Owner, Amount - 1m, ValueProp.Unpowered, cardPlay);
        }
    }
}
