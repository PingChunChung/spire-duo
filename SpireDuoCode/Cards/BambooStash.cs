namespace SpireDuo.SpireDuoCode.Cards;

public sealed class BambooStash() : PandaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BambooPower>(3m), new PowerVar<BambooNextTurnPower>(3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<BambooPower>(choiceContext, Owner.Creature, DynamicVars["BambooPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<BambooNextTurnPower>(choiceContext, Owner.Creature, DynamicVars["BambooNextTurnPower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["BambooPower"].UpgradeValueBy(1m);
        DynamicVars["BambooNextTurnPower"].UpgradeValueBy(1m);
    }
}
