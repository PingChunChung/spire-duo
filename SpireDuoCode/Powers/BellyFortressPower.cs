namespace SpireDuo.SpireDuoCode.Powers;

public sealed class BellyFortressPower : SpireDuoPower
{
    private sealed class Data
    {
        public decimal EndBlock;
    }
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override object InitInternalData() => new Data();
    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        if (creature != Owner || amount <= 0 || Owner.Block >= GetInternalData<Data>().EndBlock) return;
        Flash();
        var enemies = Owner.CombatState!.HittableEnemies;
        if (enemies.Count == 0) return;
        var target = Owner.Player!.RunState.Rng.CombatTargets.NextItem(enemies)!;
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, Amount, ValueProp.Unpowered, Owner, null);
    }
    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner)) GetInternalData<Data>().EndBlock = Owner.Block;
        return Task.CompletedTask;
    }
}
