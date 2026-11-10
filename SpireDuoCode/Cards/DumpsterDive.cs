using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DumpsterDive() : RaccoonCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await RaccoonStashStatePower.StashFromDiscard(c, Owner, this);
    }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(- 1);
}
