using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class TuckedAway() : RaccoonCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(3m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        int stashed = await RaccoonCardEffects.Stash(c, this);
        await CardPileCmd.Draw(c, 1, Owner);
        if (IsUpgraded && stashed > 0)
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, p);
    }
    protected override void OnUpgrade()
    {
    }
}
