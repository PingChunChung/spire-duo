namespace SpireDuo.SpireDuoCode.Cards;
public sealed class MaulRhythm() : PandaCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3m, ValueProp.Move), new RepeatVar(4), new PowerVar<WeakPower>(2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<WeakPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) { ArgumentNullException.ThrowIfNull(cardPlay.Target); await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(DynamicVars.Repeat.IntValue).FromCard(this).Targeting(cardPlay.Target).Execute(choiceContext); if (Owner.Creature.GetPowerAmount<StrengthPower>() > 0) await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this); }
    protected override void OnUpgrade() { DynamicVars.Damage.UpgradeValueBy(1m); DynamicVars["WeakPower"].UpgradeValueBy(1m); }
}
