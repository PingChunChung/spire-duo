using MegaCrit.Sts2.Core.CardSelection;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class TidyMind() : BlackCatCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) { await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Owner); var discarded = (await CardSelectCmd.FromHandForDiscard(context, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1), null, this)).FirstOrDefault(); if (discarded != null) await CardPileCmd.Add(discarded, PileType.Discard); await FootprintStatePower.Set(context, Owner.Creature, 1, Footprint.Skill, this); }
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1m);
}
