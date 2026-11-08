namespace SpireDuo.SpireDuoCode.Cards;

public sealed class BambooJavelin() : PandaCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9m, ValueProp.Move), new DynamicVar("Bonus", 5m), new DynamicVar("Threshold", 5m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];
    protected override bool ShouldGlowGoldInternal => Owner.Creature.GetPowerAmount<BambooPower>() >= DynamicVars["Threshold"].IntValue;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        decimal damage = DynamicVars.Damage.BaseValue;
        if (Owner.Creature.GetPowerAmount<BambooPower>() >= DynamicVars["Threshold"].IntValue) damage += DynamicVars["Bonus"].BaseValue;
        await DamageCmd.Attack(damage).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
