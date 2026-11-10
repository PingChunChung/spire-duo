using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class RainBarrelPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Task BeforeSideTurnEnd(PlayerChoiceContext c, CombatSide side, IEnumerable<Creature > participants)
    {
        int count = participants.Contains(Owner)? Owner.GetPower<RaccoonStashStatePower>()?.Count ?? 0 : 0;
        return count == 0? Task.CompletedTask : CreatureCmd.GainBlock(Owner, new BlockVar(Amount * count, ValueProp.Unpowered), null);
    }
}
