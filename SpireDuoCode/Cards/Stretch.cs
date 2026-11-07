namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Stretch() : KoalaCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrowsyPower>(3m), new CardsVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var drowsy = Owner.Creature.GetPower<DrowsyPower>();
        if (drowsy != null)
            await PowerCmd.ModifyAmount(choiceContext, drowsy, -Math.Min(drowsy.Amount, DynamicVars["DrowsyPower"].IntValue), Owner.Creature, this);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}
