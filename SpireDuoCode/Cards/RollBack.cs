using MegaCrit.Sts2.Core.CardSelection;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class RollBack() : PandaCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        var card = (await CardSelectCmd.FromCombatPile(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1), context: choiceContext, pile: PileType.Discard.GetPile(Owner), player: Owner)).FirstOrDefault();
        if (card != null)
            await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}
