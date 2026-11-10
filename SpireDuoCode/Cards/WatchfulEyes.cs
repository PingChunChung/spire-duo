using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class WatchfulEyes() : RaccoonCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, p);
        if (Owner.Creature.GetPower<RaccoonStashStatePower>()?.Count == 0)
            await CardPileCmd.Draw(c, 1, Owner);
    }
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3m);
}
