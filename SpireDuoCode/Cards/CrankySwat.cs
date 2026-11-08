namespace SpireDuo.SpireDuoCode.Cards;

public sealed class CrankySwat() : KoalaCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move), new PowerVar<VulnerablePower>(2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.FromPower<VulnerablePower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        // Waking first lets the Vulnerable apply to this hit.
        int removed = await DrowsyPower.WakeUp(choiceContext, Owner.Creature);
        if (removed > 0)
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars["VulnerablePower"].BaseValue, Owner.Creature, this);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
