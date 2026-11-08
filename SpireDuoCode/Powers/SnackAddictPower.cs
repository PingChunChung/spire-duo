namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever the owner munches Bamboo, a random enemy takes damage. Triggered from <see cref="BambooPower.Munch"/>.</summary>
public sealed class SnackAddictPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];

    internal async Task OnMunch(PlayerChoiceContext choiceContext)
    {
        var enemies = CombatState.HittableEnemies;
        if (enemies.Count == 0) return;
        var target = Owner.Player!.RunState.Rng.CombatTargets.NextItem(enemies)!;
        Flash();
        await CreatureCmd.Damage(choiceContext, target, Amount, ValueProp.Unpowered, Owner, null);
    }
}
