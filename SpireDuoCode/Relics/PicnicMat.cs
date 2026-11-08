using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Runs;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

/// <summary>Every Munch shields the owner's teammates. Triggered from <see cref="BambooPower"/> after a Munch.</summary>
[Pool(typeof(PandaRelicPool))]
public sealed class PicnicMat : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(3m, ValueProp.Unpowered)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>(), HoverTipFactory.Static(StaticHoverTip.Block)];

    public override bool IsAllowed(IRunState runState) => runState.Players.Count > 1;

    internal async Task OnMunch()
    {
        var teammates = Owner.RunState.Players.Where(p => p != Owner && p.Creature.IsAlive).ToList();
        if (teammates.Count == 0) return;
        Flash();
        foreach (var teammate in teammates)
        {
            await CreatureCmd.GainBlock(teammate.Creature, DynamicVars.Block.BaseValue, ValueProp.Unpowered, null);
        }
    }
}
