namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever the owner wakes up, every enemy takes the removed Drowsy as damage. Triggered from <see cref="DrowsyPower.WakeUp"/>.</summary>
public sealed class ShortTemperPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    internal async Task OnWake(PlayerChoiceContext choiceContext, int removed)
    {
        var enemies = CombatState.HittableEnemies.ToList();
        if (enemies.Count == 0) return;
        Flash();
        await CreatureCmd.Damage(choiceContext, enemies, removed * Amount, ValueProp.Unpowered, Owner, null);
    }
}
