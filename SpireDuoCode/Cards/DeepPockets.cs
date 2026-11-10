using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DeepPockets() : RaccoonCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        var state = await RaccoonStashStatePower.GetOrCreate(c, Owner.Creature, this);
        state.IncreaseCapacity(1);
        await PowerCmd.Apply<DeepPocketsPower>(c, Owner.Creature, 1, Owner.Creature, this);
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(- 1);
}
