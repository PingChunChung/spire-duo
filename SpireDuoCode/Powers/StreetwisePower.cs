using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class StreetwisePower : SpireDuoPower, IStashListener
{
    private readonly HashSet<CardType > _types = [];
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public async Task OnStashed(PlayerChoiceContext c, RaccoonStashStatePower s, CardModel card, CardModel source)
    {
        if (!_types.Add(card.Type))
            return;
        if (card.Type == CardType.Attack) await PlayerCmd.GainEnergy((int)
            Amount, Owner.Player!);
        else if (card.Type == CardType.Skill) await CardPileCmd.Draw(c, 2 +(int) Amount - 1, Owner.Player!);
        else if (card.Type == CardType.Power) await CreatureCmd.GainBlock(Owner, new BlockVar(12m +(Amount - 1) * 4m, ValueProp.Unpowered), null);
    }
}
