using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class HiddenCompartment() : RaccoonCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<HiddenCompartmentPower>(2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<HiddenCompartmentPower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        var state = await RaccoonStashStatePower.GetOrCreate(c, Owner.Creature, this);
        state.IncreaseCapacity(1);
        await PowerCmd.Apply<HiddenCompartmentPower>(c, Owner.Creature, DynamicVars["HiddenCompartmentPower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade() => DynamicVars["HiddenCompartmentPower"].UpgradeValueBy(1m);
}
