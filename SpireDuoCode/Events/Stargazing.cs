using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using SpireDuo.SpireDuoCode.Cards;

namespace SpireDuo.SpireDuoCode.Events;

/// <summary>Act 2, co-op only.</summary>
public sealed class Stargazing : SpireDuoEvent
{
    protected override int[] ActIndices => [1];
    protected override bool RequiresMultiplayer => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10m, ValueProp.Unblockable | ValueProp.Unpowered)];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() =>
    [
        Option(MakeAPromise, HoverTipFactory.FromCardWithCardHoverTips<Promise>()),
        Option(Wish).ThatDoesDamage(DynamicVars.Damage.BaseValue),
    ];

    private async Task MakeAPromise()
    {
        var card = Owner.RunState.CreateCard<Promise>(Owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck));
        SetEventFinished(PageDescription("MAKE_A_PROMISE"));
    }

    private async Task Wish()
    {
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars.Damage, null, null);
        await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(Owner).ToMutable(), Owner);
        SetEventFinished(PageDescription("WISH"));
    }
}
