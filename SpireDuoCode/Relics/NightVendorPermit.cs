using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;
[Pool(typeof(RaccoonRelicPool))]
public sealed class NightVendorPermit : SpireDuoRelic, IStashListener
{
    private int _turn = - 1;
    public override RelicRarity Rarity => RelicRarity.Shop;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(2m, ValueProp.Unpowered)];
    public async Task OnUnstashed(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null);
        int turn = Owner.PlayerCombatState?.TurnNumber ?? - 1;
        if (_turn == turn)
            return;
        _turn = turn;
        card.EnergyCost.SetThisTurn(Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.Local) - 1));
        Flash();
    }
}
