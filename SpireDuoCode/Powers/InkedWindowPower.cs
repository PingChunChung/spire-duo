namespace SpireDuo.SpireDuoCode.Powers;

public sealed class InkedWindowPower : FootprintListenerPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task OnFootprintLine(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line)
    {
        if (line != FootprintLine.Omen)
            return;
        var enemies = Owner.CombatState!.HittableEnemies;
        var enemy = Owner.Player!.RunState.Rng.CombatTargets.NextItem(enemies);
        if (enemy == null)
            return;
        Flash();
        await CreatureCmd.Damage(context, enemy, Amount, ValueProp.Unpowered, Owner, cardPlay.Card);
    }
}
