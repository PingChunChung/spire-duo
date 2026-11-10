using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class HiddenCompartmentPower : SpireDuoPower, IStashListener
{
    private bool _triggered;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public async Task OnStashed(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source)
    {
        if (_triggered || state.Count != state.Capacity)
            return;
        _triggered = true;
        Flash();
        await CardPileCmd.Draw(context,(int) Amount, Owner.Player!);
    }
}
