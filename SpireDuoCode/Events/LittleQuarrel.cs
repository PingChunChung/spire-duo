using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Events;
using SpireDuo.SpireDuoCode.Cards;

namespace SpireDuo.SpireDuoCode.Events;

/// <summary>Acts 2 and 3, co-op only.</summary>
public sealed class LittleQuarrel : SpireDuoEvent
{
    protected override int[] ActIndices => [1, 2];
    protected override bool RequiresMultiplayer => true;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new HealVar(10m), new DamageVar(5m, ValueProp.Unblockable | ValueProp.Unpowered)];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() =>
    [
        Option(Apologize, HoverTipFactory.FromCardWithCardHoverTips<MakeUp>()),
        Option(StormOff).ThatDoesDamage(DynamicVars.Damage.BaseValue),
    ];

    private async Task Apologize()
    {
        var card = Owner.RunState.CreateCard<MakeUp>(Owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck));
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        SetEventFinished(PageDescription("APOLOGIZE"));
    }

    private async Task StormOff()
    {
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars.Damage, null, null);
        var cards = await CardSelectCmd.FromDeckForRemoval(prefs: new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 1), player: Owner);
        await CardPileCmd.RemoveFromDeck(cards.ToList());
        SetEventFinished(PageDescription("STORM_OFF"));
    }
}
