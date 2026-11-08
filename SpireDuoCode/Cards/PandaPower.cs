namespace SpireDuo.SpireDuoCode.Cards;

public sealed class PandaPower() : PandaCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(2m), new DynamicVar("Bonus", 1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>(), HoverTipFactory.FromPower<StrengthPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal amount = DynamicVars["StrengthPower"].BaseValue;
        if (await BambooPower.Munch(choiceContext, Owner.Creature, 2, this)) amount += DynamicVars["Bonus"].BaseValue;
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, amount, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["StrengthPower"].UpgradeValueBy(1m);
    }
}
