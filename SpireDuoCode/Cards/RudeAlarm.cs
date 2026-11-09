namespace SpireDuo.SpireDuoCode.Cards;

public sealed class RudeAlarm() : KoalaCard(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2m, ValueProp.Move), new DynamicVar("Threshold", 12m), new EnergyVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), EnergyHoverTip];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int removed = await DrowsyPower.WakeUp(choiceContext, Owner.Creature);
        if (removed <= 0) return;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(removed).FromCard(this).TargetingAllOpponents(CombatState!).WithHitFx("vfx/vfx_attack_blunt").Execute(choiceContext);
        if (removed >= DynamicVars["Threshold"].IntValue) await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(1m);
}
