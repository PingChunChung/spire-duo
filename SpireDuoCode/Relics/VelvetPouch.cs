using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(BlackCatRelicPool))]

public sealed class VelvetPouch : SpireDuoRelic, IFootprintListener
{
    private bool _used;
    public override RelicRarity Rarity => RelicRarity.Common;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    public override Task BeforeCombatStart() { _used = false; return Task.CompletedTask; }
    public async Task OnFootprintCleared(PlayerChoiceContext context, CardPlay cardPlay) { if (_used) return; _used = true; Flash(); await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Owner); }
    public Task OnFootprintLine(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line) => Task.CompletedTask;
    public Task OnFootprintSetOrRotated(PlayerChoiceContext context, CardModel? source) => Task.CompletedTask;
    public Task OnCompanionResolved(PlayerChoiceContext context, CardPlay cardPlay, Creature? ally) => Task.CompletedTask;
    public Task OnEmptySlotFilled(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
}

