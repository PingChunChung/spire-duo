using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Appraise() : RaccoonCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12m, ValueProp.Move), new BlockVar(12m, ValueProp.Move), new CardsVar(2)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        var card = await RaccoonStashStatePower.Choose(c, Owner.Creature);
        if (card == null)
            return;
        if (card.Type == CardType.Attack && p.Target != null)
            await RaccoonCardEffects.Hit(c, p, DynamicVars.Damage.BaseValue);
        if (card.Type == CardType.Skill)
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.BaseValue, ValueProp.Move, p);
        if (card.Type == CardType.Power)
            await CardPileCmd.Draw(c, DynamicVars.Cards.BaseValue, Owner);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
        DynamicVars.Block.UpgradeValueBy(4m);
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}
