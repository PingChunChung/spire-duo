using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(KoalaRelicPool))]
public sealed class EucalyptusLeaf : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrowsyPower>(3m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    public override async Task BeforeCombatStart()
    {
        Flash();
        await PowerCmd.Apply<DrowsyPower>(new ThrowingPlayerChoiceContext(), Owner.Creature,
            DynamicVars["DrowsyPower"].BaseValue, Owner.Creature, null);
    }
}
