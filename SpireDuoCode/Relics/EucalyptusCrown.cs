using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace SpireDuo.SpireDuoCode.Relics;

/// <summary>Eucalyptus Leaf upgraded by Touch of Orobas (see <see cref="Patches.TouchOfOrobasPatch"/>).</summary>
[Pool(typeof(EventRelicPool))]
public sealed class EucalyptusCrown : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrowsyPower>(6m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    public override async Task BeforeCombatStart()
    {
        Flash();
        await PowerCmd.Apply<DrowsyPower>(new ThrowingPlayerChoiceContext(), Owner.Creature,
            DynamicVars["DrowsyPower"].BaseValue, Owner.Creature, null);
    }
}
