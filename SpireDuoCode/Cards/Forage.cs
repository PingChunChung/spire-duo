namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Forage() : PandaCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal cards = DynamicVars.Cards.BaseValue;
        if (await BambooPower.Munch(choiceContext, Owner.Creature, 1, this)) cards++;
        await CardPileCmd.Draw(choiceContext, cards, Owner);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}
