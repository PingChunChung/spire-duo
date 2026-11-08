namespace SpireDuo.SpireDuoCode.Cards;

public sealed class NightLight() : KoalaCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<NightLightPower>(4m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<NightLightPower>(choiceContext, Owner.Creature, DynamicVars["NightLightPower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["NightLightPower"].UpgradeValueBy(2m);
    }
}
