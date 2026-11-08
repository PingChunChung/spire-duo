using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

/// <summary>Waking up gives back half of the removed Drowsy. Triggered from <see cref="DrowsyPower.WakeUp"/>.</summary>
[Pool(typeof(KoalaRelicPool))]
public sealed class FiveMoreMinutes : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    internal async Task OnWake(PlayerChoiceContext choiceContext, int removed)
    {
        int regained = removed / 2;
        if (regained <= 0) return;
        Flash();
        await PowerCmd.Apply<DrowsyPower>(choiceContext, Owner.Creature, regained, Owner.Creature, null);
    }
}
