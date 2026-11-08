namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Tantrum() : KoalaCard(1, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2m, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int removed = await DrowsyPower.WakeUp(choiceContext, Owner.Creature);
        if (removed <= 0) return;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(removed).FromCard(this).TargetingRandomOpponents(CombatState!)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
    }
}
