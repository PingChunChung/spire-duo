namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever the owner gains Drowsy, a random enemy takes that much damage.</summary>
public sealed class DreamlandPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power is not DrowsyPower || power.Owner != Owner || amount <= 0) return;
        var enemies = CombatState.HittableEnemies;
        if (enemies.Count == 0) return;
        var target = Owner.Player!.RunState.Rng.CombatTargets.NextItem(enemies)!;
        Flash();
        await CreatureCmd.Damage(choiceContext, target, amount * Amount, ValueProp.Unpowered, Owner, null);
    }
}
