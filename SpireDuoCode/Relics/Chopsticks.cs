using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

/// <summary>When a Munch can't be paid, gain Bamboo instead. Triggered from <see cref="BambooPower.Munch"/>.</summary>
[Pool(typeof(PandaRelicPool))]
public sealed class Chopsticks : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BambooPower>(1m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];

    internal async Task OnMunchFailed(PlayerChoiceContext choiceContext)
    {
        Flash();
        await PowerCmd.Apply<BambooPower>(choiceContext, Owner.Creature, DynamicVars["BambooPower"].BaseValue, Owner.Creature, null);
    }
}
