namespace SpireDuo.SpireDuoCode.Cards;

public sealed class GrumpyMorning() : KoalaCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<GrumpyMorningPower>(2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.FromPower<StrengthPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<GrumpyMorningPower>(choiceContext, Owner.Creature, DynamicVars["GrumpyMorningPower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["GrumpyMorningPower"].UpgradeValueBy(1m);
    }
}
