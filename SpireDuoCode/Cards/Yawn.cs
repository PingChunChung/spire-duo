namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Yawn() : KoalaCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrowsyPower>(1m), new PowerVar<WeakPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.FromPower<WeakPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<DrowsyPower>(choiceContext, Owner.Creature, DynamicVars["DrowsyPower"].BaseValue, Owner.Creature, this);
        foreach (var enemy in CombatState!.HittableEnemies.ToList())
            await PowerCmd.Apply<WeakPower>(choiceContext, enemy, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["DrowsyPower"].UpgradeValueBy(1m);
    }
}
