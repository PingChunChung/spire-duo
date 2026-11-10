using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class NightLedgerPower : SpireDuoPower, IStashListener
{
    private bool _triggered;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public async Task OnSpent(PlayerChoiceContext c, RaccoonStashStatePower s, CardModel card, CardModel source)
    {
        if (_triggered || RaccoonStashStatePower.CostValue(card) < 3)
            return;
        _triggered = true;
        await PowerCmd.Apply<StrengthPower>(c, Owner, 1, Owner, source);
    }
}
