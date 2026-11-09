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

[Pool(typeof(BlackCatRelicPool))]
public sealed class WindowsillToken : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    public override async Task BeforeCombatStart()
    {
        if (PileType.Deck.GetPile(Owner).Cards.Count(card => card.Type == CardType.Skill) < 8) return;
        Flash();
        await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1, Owner.Creature, null);
    }
}

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

[Pool(typeof(BlackCatRelicPool))]
public sealed class GutterCrown : SpireDuoRelic, IFootprintListener
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3m, ValueProp.Unpowered)];
    public async Task OnFootprintLine(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line)
    {
        if (line != FootprintLine.Pounce) return;
        Flash();
        foreach (var enemy in Owner.Creature.CombatState!.HittableEnemies)
            await CreatureCmd.Damage(context, enemy, DynamicVars.Damage.BaseValue, ValueProp.Unpowered, Owner.Creature, cardPlay.Card);
    }
    public Task OnFootprintSetOrRotated(PlayerChoiceContext context, CardModel? source) => Task.CompletedTask;
    public Task OnCompanionResolved(PlayerChoiceContext context, CardPlay cardPlay, Creature? ally) => Task.CompletedTask;
    public Task OnEmptySlotFilled(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
    public Task OnFootprintCleared(PlayerChoiceContext context, CardPlay cardPlay) => Task.CompletedTask;
}

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
