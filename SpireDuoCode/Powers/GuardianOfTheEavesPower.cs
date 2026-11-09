namespace SpireDuo.SpireDuoCode.Powers;

public sealed class GuardianOfTheEavesPower : SpireDuoPower
{
    private int _turn = -1;
    private Creature? _protectedAlly;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyHpLostBeforeOsty(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? source)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1;
        if (turn == _turn || target.Side != Owner.Side || target == Owner || amount <= 0m)
            return amount;

        _turn = turn;
        _protectedAlly = target;
        return Math.Max(0m, amount - Amount);
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext context, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? source)
    {
        if (target != _protectedAlly)
            return;

        _protectedAlly = null;
        Flash();
        Creature? enemy = Owner.Player!.RunState.Rng.CombatTargets.NextItem(Owner.CombatState!.HittableEnemies);
        if (enemy != null)
            await CreatureCmd.Damage(context, enemy, Amount, ValueProp.Unpowered, Owner, source);
    }
}
