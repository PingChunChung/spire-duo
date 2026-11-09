using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

public sealed class EbonCalendarPower : FootprintListenerPower
{
    private int _turn = -1;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task OnFootprintLine(PlayerChoiceContext context, CardPlay cardPlay, FootprintLine line)
    {
        int turn = Owner.Player?.PlayerCombatState?.TurnNumber ?? -1;
        if (line != FootprintLine.Omen || turn == _turn)
            return;

        _turn = turn;
        CardModel? card = (await CardSelectCmd.FromCombatPile(
            prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1),
            context: context,
            pile: PileType.Discard.GetPile(Owner.Player!),
            player: Owner.Player!)).FirstOrDefault();
        if (card == null)
            return;

        if (Amount > 1)
            card.EnergyCost.SetUntilPlayed(0, true);
        await CardPileCmd.Add(card, PileType.Hand);
    }
}
