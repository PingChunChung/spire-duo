using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class RagsToRichesPower : SpireDuoPower, IStashListener
{
    private int _turn = - 1;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public async Task OnSpent(PlayerChoiceContext c, RaccoonStashStatePower s, CardModel card, CardModel source)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? - 1;
        if (_turn == turn)
            return;
        _turn = turn;
        var enemies = Owner.CombatState!.HittableEnemies;
        var target = Owner.Player!.RunState.Rng.CombatTargets.NextItem(enemies);
        if (target != null)
            await CreatureCmd.Damage(c, target, Amount, ValueProp.Unpowered, Owner, source);
        await CreatureCmd.GainBlock(Owner, new BlockVar(Amount, ValueProp.Unpowered), null);
    }
}
