namespace SpireDuo.SpireDuoCode.Powers;

public sealed class HuntersPausePower : FootprintListenerPower
{
    private int _turn = -1;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task OnFootprintLine(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1;
        if (line != FootprintLine.Pounce || turn == _turn)
            return;

        _turn = turn;
        Creature? target = cardPlay.Target is { IsDead: false } chosen && chosen.Side != Owner.Side
            ? chosen
            : Owner.CombatState?.HittableEnemies.MaxBy(enemy => enemy.CurrentHp);
        if (target == null)
            return;

        Flash();
        await DamageCmd.Attack(Amount).FromCard(cardPlay.Card).Targeting(target).WithHitFx("vfx/vfx_attack_slash").Execute(context);
    }
}
