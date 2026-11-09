namespace SpireDuo.SpireDuoCode.Cards;

public sealed class NightWatch() : KoalaCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<NightWatchPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) =>
        await PowerCmd.Apply<NightWatchPower>(choiceContext, Owner.Creature, DynamicVars["NightWatchPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["NightWatchPower"].UpgradeValueBy(1m);
}
