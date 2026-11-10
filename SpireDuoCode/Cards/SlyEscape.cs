using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SlyEscape() : RaccoonCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(3m), new EnergyVar(2), new CardsVar(2)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await CreatureCmd.Damage(c, Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        if (await RaccoonStashStatePower.Spend(c, Owner.Creature, this) == null)
            return;
        await PlayerCmd.GainEnergy((int) DynamicVars.Energy.BaseValue, Owner);
        await CardPileCmd.Draw(c, DynamicVars.Cards.BaseValue, Owner);
    }
    protected override void OnUpgrade() => DynamicVars.HpLoss.UpgradeValueBy(- 1m);
}
