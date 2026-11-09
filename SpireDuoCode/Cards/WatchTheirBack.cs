namespace SpireDuo.SpireDuoCode.Cards;

public sealed class WatchTheirBack() : BlackCatCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("AllyBlock", 10m)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        if (play.Target is { IsDead: false } ally)
            await CreatureCmd.GainBlock(ally, DynamicVars["AllyBlock"].BaseValue, ValueProp.Move, play);
        await FootprintStatePower.Lock(context, Owner.Creature, 0, this);
        await FootprintStatePower.Lock(context, Owner.Creature, 1, this);
    }
    protected override void OnUpgrade() => DynamicVars["AllyBlock"].UpgradeValueBy(3m);
}
