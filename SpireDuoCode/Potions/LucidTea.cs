using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Localization;

namespace SpireDuo.SpireDuoCode.Potions;

public sealed class LucidTea : KoalaPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrowsyPower>(4m)];

    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain), HoverTipFactory.FromPower<DrowsyPower>()];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        var card = (await CardSelectCmd.FromHand(
            prefs: new CardSelectorPrefs(new LocString("potions", "SPIREDUO-LUCID_TEA.selectionScreenPrompt"), 1),
            context: choiceContext,
            player: Owner,
            filter: c => c.Keywords.Contains(CardKeyword.Retain),
            source: this)).FirstOrDefault();
        card?.EnergyCost.SetThisTurn(0);
        await PowerCmd.Apply<DrowsyPower>(choiceContext, Owner.Creature, DynamicVars["DrowsyPower"].BaseValue, Owner.Creature, null);
    }
}
