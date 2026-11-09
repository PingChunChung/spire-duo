namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>At the end of the owner's turn, a random enemy takes damage equal to half the owner's Block per stack.</summary>
public sealed class RollingThunderPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner)) return;
        for (int i = 0; i < Amount; i++)
        {
            int block = Owner.Block / 2;
            if (block <= 0) return;
            var enemies = CombatState.HittableEnemies;
            if (enemies.Count == 0) return;
            var target = Owner.Player!.RunState.Rng.CombatTargets.NextItem(enemies)!;
            Flash();
            await CreatureCmd.Damage(choiceContext, target, block, ValueProp.Unpowered, Owner, null);
        }
    }
}
