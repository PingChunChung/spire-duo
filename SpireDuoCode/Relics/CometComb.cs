using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(BlackCatRelicPool))]

public sealed class CometComb : SpireDuoRelic, IFootprintListener
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(3m, ValueProp.Unpowered)];
    public async Task OnFootprintLine(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line) { if (line != FootprintLine.Groom) return; Flash(); await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay); await PowerCmd.Apply<DexterityPower>(context, Owner.Creature, 1, Owner.Creature, cardPlay.Card); }
    public Task OnFootprintSetOrRotated(PlayerChoiceContext context, CardModel? source) => Task.CompletedTask;
    public Task OnCompanionResolved(PlayerChoiceContext context, CardPlay cardPlay, Creature? ally) => Task.CompletedTask;
    public Task OnEmptySlotFilled(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
    public Task OnFootprintCleared(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
}

