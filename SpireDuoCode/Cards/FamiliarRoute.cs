namespace SpireDuo.SpireDuoCode.Cards;

public sealed class FamiliarRoute() : BlackCatCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        var state = await FootprintStatePower.GetOrCreate(context, Owner.Creature, this);
        int source = Array.FindLastIndex(state.Slots.ToArray(), footprint => footprint != Footprint.Empty);
        int target = Enumerable.Range(0, state.Slots.Count).FirstOrDefault(index => state.Slots[index] == Footprint.Empty && !state.Locked[index], -1);
        if (source >= 0 && target >= 0)
            await state.Set(context, target, state.Slots[source], this);
        if (IsUpgraded)
            await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Owner);
    }
    protected override void OnUpgrade() { }
}
