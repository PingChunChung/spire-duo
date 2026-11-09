namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SleepwalkersLunge() : KoalaCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5m, ValueProp.Move), new RepeatVar(2), new PowerVar<VulnerablePower>(2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(DynamicVars.Repeat.IntValue).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt").Execute(choiceContext);
        if (DrowsyPower.WokeThisTurn(Owner.Creature))
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars["VulnerablePower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
        DynamicVars["VulnerablePower"].UpgradeValueBy(1m);
    }
}
