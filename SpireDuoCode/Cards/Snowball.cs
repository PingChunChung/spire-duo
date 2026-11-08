namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Snowball() : PandaCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move), new DynamicVar("Increase", 4m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
        decimal increase = DynamicVars["Increase"].BaseValue;
        if (await BambooPower.Munch(choiceContext, Owner.Creature, 1, this)) increase *= 2;
        DynamicVars.Damage.BaseValue += increase;
    }
    protected override void OnUpgrade()
    {
        DynamicVars["Increase"].UpgradeValueBy(2m);
    }
}
