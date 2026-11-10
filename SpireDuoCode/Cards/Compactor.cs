using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class Compactor() : RaccoonCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(9m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        int count = 0;
        while (count < 3 && await RaccoonStashStatePower.Spend(c, Owner.Creature, this)
            != null)
        {
            count++;
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.BaseValue +(IsUpgraded? 2m : 0m), ValueProp.Move, p);
            await CardPileCmd.Draw(c, 1, Owner);
        }
    }
    protected override void OnUpgrade()
    {
    }
}
