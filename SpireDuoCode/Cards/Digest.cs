namespace SpireDuo.SpireDuoCode.Cards;
public sealed class Digest() : PandaCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DigestPower>(5m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FullPower>(), HoverTipFactory.Static(StaticHoverTip.Block)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) => await PowerCmd.Apply<DigestPower>(choiceContext, Owner.Creature, DynamicVars["DigestPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["DigestPower"].UpgradeValueBy(2m);
}
