namespace SpireDuo.SpireDuoCode.Cards;

public sealed class BellyBounce() : PandaCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BellyBouncePower>(4m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<BellyBouncePower>(choiceContext, Owner.Creature, DynamicVars["BellyBouncePower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["BellyBouncePower"].UpgradeValueBy(2m);
    }
}
