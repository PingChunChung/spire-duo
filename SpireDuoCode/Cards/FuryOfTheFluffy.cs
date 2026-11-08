namespace SpireDuo.SpireDuoCode.Cards;

public sealed class FuryOfTheFluffy() : PandaCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FuryOfTheFluffyPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FuryOfTheFluffyPower>(choiceContext, Owner.Creature, DynamicVars["FuryOfTheFluffyPower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["FuryOfTheFluffyPower"].UpgradeValueBy(1m);
    }
}
