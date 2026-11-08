namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SlumberParty() : KoalaCard(2, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DrowsyPower>(5m), new CardsVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (var creature in CombatState!.GetTeammatesOf(Owner.Creature).Where(c => c.IsAlive && c.IsPlayer).ToList())
        {
            await PowerCmd.Apply<DrowsyPower>(choiceContext, creature, DynamicVars["DrowsyPower"].BaseValue, Owner.Creature, this);
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, creature.Player!);
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars["DrowsyPower"].UpgradeValueBy(2m);
    }
}
