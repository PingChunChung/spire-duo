using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(BlackCatRelicPool))]

public sealed class LanternOfOneWindow : SpireDuoRelic, IFootprintListener
{
    private bool _used;
    public override RelicRarity Rarity => RelicRarity.Rare;
    public override Task BeforeCombatStart() { _used = false; return Task.CompletedTask; }
    public async Task OnCompanionResolved(PlayerChoiceContext context, CardPlay cardPlay, Creature? ally) { if (_used || ally?.Player == null || ally.IsDead) return; _used = true; Flash(); await CardPileCmd.Draw(context, 1, Owner); await CardPileCmd.Draw(context, 1, ally.Player); }
    public Task OnFootprintLine(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line) => Task.CompletedTask;
    public Task OnFootprintSetOrRotated(PlayerChoiceContext context, CardModel? source) => Task.CompletedTask;
    public Task OnEmptySlotFilled(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
    public Task OnFootprintCleared(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
}

