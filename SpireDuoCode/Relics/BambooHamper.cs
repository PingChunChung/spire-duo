using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace SpireDuo.SpireDuoCode.Relics;

/// <summary>Bamboo Basket upgraded by Touch of Orobas (see <see cref="Patches.TouchOfOrobasPatch"/>).</summary>
[Pool(typeof(EventRelicPool))]
public sealed class BambooHamper : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BambooPower>(6m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];

    public override async Task BeforeCombatStart()
    {
        Flash();
        await PowerCmd.Apply<BambooPower>(new ThrowingPlayerChoiceContext(), Owner.Creature,
            DynamicVars["BambooPower"].BaseValue, Owner.Creature, null);
    }
}
