namespace SpireDuo.SpireDuoCode.Cards;
public sealed class TailFeint() : BlackCatCard(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { ArgumentNullException.ThrowIfNull(p.Target); await PowerCmd.Apply<WeakPower>(c, p.Target, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this); }
    protected override void OnUpgrade() => DynamicVars["WeakPower"].UpgradeValueBy(1m);
}
