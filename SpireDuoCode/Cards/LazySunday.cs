namespace SpireDuo.SpireDuoCode.Cards;

/// <summary>Koala's Dusty Tome card.</summary>
public sealed class LazySunday() : KoalaCard(3, CardType.Power, CardRarity.Ancient, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<LazySundayPower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<LazySundayPower>(choiceContext, Owner.Creature, DynamicVars["LazySundayPower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
