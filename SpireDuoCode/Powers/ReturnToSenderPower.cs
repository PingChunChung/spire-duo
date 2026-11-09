namespace SpireDuo.SpireDuoCode.Powers;

public sealed class ReturnToSenderPower : SpireDuoPower
{
    private sealed class Data { public int Turn; }
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override object InitInternalData() => new Data { Turn = -1 };
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        var data = GetInternalData<Data>();
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? 0;
        if (target != Owner || data.Turn == turn || result.UnblockedDamage <= 0 || dealer == null || dealer.Side == Owner.Side || !props.IsPoweredAttack()) return;
        data.Turn = turn;
        decimal reflected = Math.Floor(result.UnblockedDamage * Amount / 100m);
        if (reflected <= 0) return;
        Flash();
        await CreatureCmd.GainBlock(Owner, reflected, ValueProp.Unpowered, null);
        await CreatureCmd.Damage(choiceContext, dealer, reflected, ValueProp.Unpowered, Owner, null);
    }
}
