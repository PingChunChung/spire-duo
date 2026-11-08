namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SnackAddict() : PandaCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SnackAddictPower>(4m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<SnackAddictPower>(choiceContext, Owner.Creature, DynamicVars["SnackAddictPower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["SnackAddictPower"].UpgradeValueBy(2m);
    }
}
