using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class TakeInventory() : RaccoonCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(3m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        int n = Owner.Creature.GetPower<RaccoonStashStatePower>()?.Count ?? 0;
        await CreatureCmd.GainBlock(Owner.Creature, new BlockVar(n * DynamicVars.Block.BaseValue, ValueProp.Move), p);
    }
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(1m);
}
