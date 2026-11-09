using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SilentHousehold() : BlackCatCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(7m, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        IEnumerable<CardModel> selected = await CardSelectCmd.FromHand(
            context,
            Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, 0, int.MaxValue),
            card => card != this,
            this);
        foreach (CardModel card in selected)
        {
            await CardCmd.Exhaust(context, card);
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }
    }

    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(2m);
}
