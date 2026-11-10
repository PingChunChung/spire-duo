using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class QuickWash() : RaccoonCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        var debuff = Owner.Creature.Powers.FirstOrDefault(x => x.Type == PowerType.Debuff);
        if (debuff != null)
            await PowerCmd.Remove(debuff);
        if (RaccoonStashStatePower.Has(Owner.Creature, x => x.Type == CardType.Skill))
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, p);
    }
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3m);
}
