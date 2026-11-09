namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DreamSmash() : KoalaCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(20m, ValueProp.Move), new PowerVar<DrowsyPower>(5m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];
    protected override bool IsPlayable => Owner.Creature.GetPowerAmount<DrowsyPower>() >= DynamicVars["DrowsyPower"].IntValue;
    protected override bool ShouldGlowGoldInternal => IsPlayable;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6m);
        DynamicVars["DrowsyPower"].UpgradeValueBy(2m);
    }
}
