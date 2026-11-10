using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;

namespace SpireDuo.SpireDuoCode.Relics;
[Pool(typeof(SharedRelicPool))]
public sealed class SpareTote : SpireDuoRelic, IStashListener
{
    private bool _used;
    public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8m, ValueProp.Unpowered), new CardsVar(1)];
    public override bool IsAllowed(IRunState runState) => runState.Players.Count > 1;
    public override Task BeforeCombatStart()
    {
        _used = false;
        return Task.CompletedTask;
    }
    public async Task OnStashed(PlayerChoiceContext context, RaccoonStashStatePower state, CardModel card, CardModel source)
    {
        if (_used)
            return;
        var ally = Owner.RunState.Players.Where(player => player != Owner && player.Creature.IsAlive).OrderBy(player => Owner.RunState.GetPlayerSlotIndex(player)).FirstOrDefault();
        if (ally == null)
            return;
        _used = true;
        Flash();
        await CreatureCmd.GainBlock(ally.Creature, DynamicVars.Block, null);
        await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, ally);
    }
}
