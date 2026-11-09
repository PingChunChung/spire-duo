namespace SpireDuo.SpireDuoCode.Cards;
public sealed class Downhill() : PandaCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move), new RepeatVar(2), new DynamicVar("PerRoll", 2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<RollPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) { ArgumentNullException.ThrowIfNull(cardPlay.Target); int roll = await RollPower.SpendAll(Owner.Creature); decimal damage = DynamicVars.Damage.BaseValue + roll * DynamicVars["PerRoll"].BaseValue; await DamageCmd.Attack(damage).WithHitCount(DynamicVars.Repeat.IntValue).FromCard(this).Targeting(cardPlay.Target).Execute(choiceContext); }
    protected override void OnUpgrade() { DynamicVars.Damage.UpgradeValueBy(2m); DynamicVars["PerRoll"].UpgradeValueBy(1m); }
}
