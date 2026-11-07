namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>At the end of the owner's turn, a random enemy takes damage equal to the owner's Drowsy.</summary>
public sealed class SnoringPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner)) return;
        int drowsy = Owner.GetPowerAmount<DrowsyPower>();
        if (drowsy <= 0) return;
        for (int i = 0; i < Amount; i++)
        {
            var enemies = CombatState.HittableEnemies;
            if (enemies.Count == 0) return;
            var target = Owner.Player!.RunState.Rng.CombatTargets.NextItem(enemies)!;
            Flash();
            await CreatureCmd.Damage(choiceContext, target, drowsy, ValueProp.Unpowered, Owner, null);
        }
    }
}
