using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(BlackCatRelicPool))]

public sealed class CuriosityString : SpireDuoRelic, IFootprintListener
{
    public override RelicRarity Rarity => RelicRarity.Shop;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(2m, ValueProp.Unpowered)];
    public async Task OnFootprintSetOrRotated(PlayerChoiceContext context, CardModel? source) { Flash(); await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null); }
    public Task OnFootprintLine(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line) => Task.CompletedTask;
    public Task OnCompanionResolved(PlayerChoiceContext context, CardPlay cardPlay, Creature? ally) => Task.CompletedTask;
    public Task OnEmptySlotFilled(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
    public Task OnFootprintCleared(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
}
