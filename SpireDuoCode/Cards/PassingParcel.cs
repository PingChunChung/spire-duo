using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class PassingParcel() : RaccoonCard(1, CardType.Skill, CardRarity.Common, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(3m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        if (await RaccoonStashStatePower.Spend(c, Owner.Creature, this)
            != null && p.Target?.Player != null)
        {
            await CardPileCmd.Draw(c, 2, p.Target.Player);
            if (IsUpgraded)
                await CreatureCmd.GainBlock(p.Target, DynamicVars.Block, p);
        }
    }
    protected override void OnUpgrade()
    {
    }
}
