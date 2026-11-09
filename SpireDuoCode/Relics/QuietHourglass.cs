using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(BlackCatRelicPool))]

public sealed class QuietHourglass : SpireDuoRelic, IFootprintListener
{
    private bool _used;
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2)];
    public override Task BeforeCombatStart() { _used = false; return Task.CompletedTask; }
    public async Task OnFootprintLine(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line) { if (_used || line != FootprintLine.Omen) return; _used = true; Flash(); await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner); }
    public Task OnFootprintSetOrRotated(PlayerChoiceContext context, CardModel? source) => Task.CompletedTask;
    public Task OnCompanionResolved(PlayerChoiceContext context, CardPlay cardPlay, Creature? ally) => Task.CompletedTask;
    public Task OnEmptySlotFilled(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
    public Task OnFootprintCleared(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
}

