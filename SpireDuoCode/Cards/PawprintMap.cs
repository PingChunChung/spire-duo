namespace SpireDuo.SpireDuoCode.Cards;

public sealed class PawprintMap() : BlackCatCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) { var state = await FootprintStatePower.GetOrCreate(context, Owner.Creature, this); await state.Set(context, 0, Footprint.Attack, this); await state.Set(context, 2, Footprint.Attack, this); }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
