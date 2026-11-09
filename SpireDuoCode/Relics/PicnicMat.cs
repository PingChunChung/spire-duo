using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Runs;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

/// <summary>Every Munch shields the owner's teammates. Triggered from <see cref="BambooPower"/> after a Munch.</summary>
[Pool(typeof(PandaRelicPool))]
public sealed class PicnicMat : SpireDuoRelic
{
    private int _lastTriggeredTurn = -1;
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(4m, ValueProp.Unpowered)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>(), HoverTipFactory.Static(StaticHoverTip.Block)];

    public override bool IsAllowed(IRunState runState) => runState.Players.Count > 1;

    internal async Task OnMunch()
    {
        int turn = Owner.PlayerCombatState?.TurnNumber ?? 0;
        if (_lastTriggeredTurn == turn) return;
        _lastTriggeredTurn = turn;
        var teammates = Owner.RunState.Players.Where(p => p != Owner && p.Creature.IsAlive)
            .OrderBy(p => p.Creature.Block).ThenBy(p => Owner.RunState.GetPlayerSlotIndex(p)).ToList();
        if (teammates.Count == 0) return;
        Flash();
        await CreatureCmd.GainBlock(teammates[0].Creature, DynamicVars.Block.BaseValue, ValueProp.Unpowered, null);
    }
}
