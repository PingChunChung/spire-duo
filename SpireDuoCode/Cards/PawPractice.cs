namespace SpireDuo.SpireDuoCode.Cards;
public sealed class PawPractice() : PandaCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4m, ValueProp.Move), new RepeatVar(3)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) { ArgumentNullException.ThrowIfNull(cardPlay.Target); await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(DynamicVars.Repeat.IntValue).FromCard(this).Targeting(cardPlay.Target).Execute(choiceContext); }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(1m);
}
