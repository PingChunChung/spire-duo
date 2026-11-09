using MegaCrit.Sts2.Core.CardSelection;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Foretell() : BlackCatCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        var drawPile = PileType.Draw.GetPile(Owner);
        var options = drawPile.Cards.TakeLast(3).ToList();
        var selected = (await CardSelectCmd.FromSimpleGrid(context, options, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1))).FirstOrDefault();
        if (selected != null)
            await CardPileCmd.Add(selected, PileType.Hand);

        await (await FootprintStatePower.GetOrCreate(context, Owner.Creature, this)).RotateRight(context, this);
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
