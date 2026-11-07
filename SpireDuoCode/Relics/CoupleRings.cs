using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(SharedRelicPool))]
public sealed class CoupleRings : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5m, ValueProp.Unpowered)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    public override bool IsAllowed(IRunState runState) => runState.Players.Count > 1;

    public override async Task BeforeCombatStart()
    {
        Flash();
        foreach (var player in Owner.RunState.Players.Where(p => p.Creature.IsAlive).ToList())
        {
            await CreatureCmd.GainBlock(player.Creature, DynamicVars.Block.BaseValue, ValueProp.Unpowered, null);
        }
    }
}
