using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(PandaRelicPool))]
public sealed class LunchBox : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Shop;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BambooPower>(4m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];

    public override async Task BeforeCombatStart()
    {
        Flash();
        await PowerCmd.Apply<BambooPower>(new ThrowingPlayerChoiceContext(), Owner.Creature,
            DynamicVars["BambooPower"].BaseValue, Owner.Creature, null);
    }
}
