using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class VelvetFate() : BlackCatCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await CreatureCmd.Damage(
            context,
            Owner.Creature,
            IsUpgraded ? 3m : 4m,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,
            this);

        CardModel? selected = (await CardSelectCmd.FromHand(
            context,
            Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, 1),
            card => card.IsUpgradable,
            this)).FirstOrDefault();
        if (selected != null)
            CardCmd.Upgrade(selected);
    }

    protected override void OnUpgrade()
    {
    }
}
