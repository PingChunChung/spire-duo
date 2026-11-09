namespace SpireDuo.SpireDuoCode.Cards;
public sealed class QuietRoutine() : BlackCatCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<DrawCardsNextTurnPower>(c, Owner.Creature, DynamicVars.Cards.BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1m);
}
