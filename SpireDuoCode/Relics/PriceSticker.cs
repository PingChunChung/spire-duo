using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;
[Pool(typeof(RaccoonRelicPool))]
public sealed class PriceSticker : SpireDuoRelic, IStashListener
{
    private bool _used;
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2)];
    public override Task BeforeCombatStart()
    {
        _used = false;
        return Task.CompletedTask;
    }
    public async Task OnUnstashed(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source)
    {
        if (_used || RaccoonStashStatePower.CostValue(card) != 3)
            return;
        _used = true;
        Flash();
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
}
