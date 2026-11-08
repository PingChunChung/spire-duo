namespace SpireDuo.SpireDuoCode.Cards;

public sealed class GroggyCombo() : KoalaCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move), new PowerVar<DrowsyPower>(3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];
    protected override bool ShouldGlowGoldInternal => Owner.Creature.GetPowerAmount<DrowsyPower>() >= DynamicVars["DrowsyPower"].IntValue;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        int hits = 1;
        int cost = DynamicVars["DrowsyPower"].IntValue;
        var drowsy = Owner.Creature.GetPower<DrowsyPower>();
        if (drowsy != null && drowsy.Amount >= cost)
        {
            await PowerCmd.ModifyAmount(choiceContext, drowsy, -cost, Owner.Creature, this);
            hits++;
        }
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(hits).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
