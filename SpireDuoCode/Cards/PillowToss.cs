namespace SpireDuo.SpireDuoCode.Cards;

public sealed class PillowToss() : KoalaCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move), new PowerVar<DrowsyPower>(3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        var enemies = CombatState!.HittableEnemies;
        if (enemies.Count > 0)
        {
            var enemy = Owner.RunState.Rng.CombatTargets.NextItem(enemies)!;
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(enemy)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);
        }
        await PowerCmd.Apply<DrowsyPower>(choiceContext, cardPlay.Target, DynamicVars["DrowsyPower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
        DynamicVars["DrowsyPower"].UpgradeValueBy(1m);
    }
}
