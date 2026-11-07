using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

/// <summary>Whenever the owner wakes up, draw cards. Triggered from <see cref="DrowsyPower.WakeUp"/>.</summary>
[Pool(typeof(KoalaRelicPool))]
public sealed class AlarmClock : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    internal async Task OnWake(PlayerChoiceContext choiceContext)
    {
        Flash();
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }
}
