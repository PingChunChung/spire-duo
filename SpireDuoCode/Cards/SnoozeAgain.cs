namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SnoozeAgain() : KoalaCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Per", 5m), new EnergyVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), EnergyHoverTip];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int removed = await DrowsyPower.WakeUp(choiceContext, Owner.Creature);
        if (removed > 0) await PlayerCmd.GainEnergy(removed / DynamicVars["Per"].IntValue * DynamicVars.Energy.IntValue, Owner);
    }
    protected override void OnUpgrade() => DynamicVars["Per"].UpgradeValueBy(-1m);
}
