using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class MoonlitHoard() : RaccoonCard(3, CardType.Power, CardRarity.Ancient, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<MoonlitHoardPower>(8m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<MoonlitHoardPower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        var state = await RaccoonStashStatePower.GetOrCreate(c, Owner.Creature, this);
        state.IncreaseCapacity(2);
        await PowerCmd.Apply<MoonlitHoardPower>(c, Owner.Creature, DynamicVars["MoonlitHoardPower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade() => DynamicVars["MoonlitHoardPower"].UpgradeValueBy(4m);
}
