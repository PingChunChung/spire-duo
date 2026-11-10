using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;
[Pool(typeof(RaccoonRelicPool))]
public sealed class SilverCarabiner : SpireDuoRelic, IStashListener
{
    private int _turn = - 1;
    public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(4m, ValueProp.Unpowered), new CardsVar(1)];
    public async Task OnSpent(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source)
    {
        int turn = Owner.PlayerCombatState?.TurnNumber ?? - 1;
        if (_turn == turn)
            return;
        _turn = turn;
        Flash();
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null);
        await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Owner);
    }
}
