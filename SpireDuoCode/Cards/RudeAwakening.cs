namespace SpireDuo.SpireDuoCode.Cards;

public sealed class RudeAwakening() : KoalaCard(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CalculationBaseVar(0m), new ExtraDamageVar(2m), new CalculatedDamageVar(ValueProp.Move).WithMultiplier((card, _) => card.Owner.Creature.GetPowerAmount<DrowsyPower>())];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).TargetingAllOpponents(CombatState!)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
        await DrowsyPower.WakeUp(Owner.Creature);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraDamage.UpgradeValueBy(1m);
    }
}
