namespace SpireDuo.SpireDuoCode.Cards;

public sealed class ComfortEating() : PandaCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int eaten = await BambooPower.MunchAll(choiceContext, Owner.Creature, this);
        if (eaten > 0)
            await CreatureCmd.Heal(Owner.Creature, eaten * DynamicVars.Heal.BaseValue);
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
