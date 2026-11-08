namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Whenever a teammate plays an Attack, a random enemy takes damage.</summary>
public sealed class PartnerInCrimePower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay.Card;
        if (card.Type != CardType.Attack || card.Owner == Owner.Player || card.Owner.Creature.Side != Owner.Side) return;
        var enemies = CombatState.HittableEnemies;
        if (enemies.Count == 0) return;
        var target = Owner.Player!.RunState.Rng.CombatTargets.NextItem(enemies)!;
        Flash();
        await CreatureCmd.Damage(choiceContext, target, Amount, ValueProp.Unpowered, Owner, null);
    }
}
