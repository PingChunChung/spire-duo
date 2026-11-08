using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Runs;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(KoalaRelicPool))]
public sealed class SharedBlanket : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.Static(StaticHoverTip.Block)];

    public override bool IsAllowed(IRunState runState) => runState.Players.Count > 1;

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner.Creature)) return;
        int block = Owner.Creature.GetPowerAmount<DrowsyPower>() / 2;
        if (block <= 0) return;
        var teammates = Owner.RunState.Players.Where(p => p != Owner && p.Creature.IsAlive).ToList();
        if (teammates.Count == 0) return;
        Flash();
        foreach (var teammate in teammates)
        {
            await CreatureCmd.GainBlock(teammate.Creature, block, ValueProp.Unpowered, null);
        }
    }
}
