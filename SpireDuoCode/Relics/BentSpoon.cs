using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;
[Pool(typeof(RaccoonRelicPool))]
public sealed class BentSpoon : SpireDuoRelic, IStashListener
{
    private bool _used;
    public override RelicRarity Rarity => RelicRarity.Common;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    public override Task BeforeCombatStart()
    {
        _used = false;
        return Task.CompletedTask;
    }
    public async Task OnSpent(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source)
    {
        if (_used)
            return;
        _used = true;
        Flash();
        await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Owner);
    }
}
