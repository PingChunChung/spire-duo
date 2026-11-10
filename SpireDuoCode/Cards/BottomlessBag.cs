using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class BottomlessBag() : RaccoonCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        var state = await RaccoonStashStatePower.GetOrCreate(c, Owner.Creature, this);
        state.IncreaseCapacity(2);
        await PowerCmd.Apply<BottomlessBagPower>(c, Owner.Creature, 1, Owner.Creature, this);
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(- 1);
}
