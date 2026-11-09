using MegaCrit.Sts2.Core.CardSelection;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SweepAside() : BlackCatCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(11m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) { await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play); var selected = (await CardSelectCmd.FromHand(context, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1), card => card != this, this)).FirstOrDefault(); if (selected != null) await CardCmd.Exhaust(context, selected); }
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3m);
}
