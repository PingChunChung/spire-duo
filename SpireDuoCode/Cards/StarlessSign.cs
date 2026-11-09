namespace SpireDuo.SpireDuoCode.Cards;
public sealed class StarlessSign() : BlackCatCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { await FootprintStatePower.Set(c, Owner.Creature, 2, Footprint.Power, this); await CardPileCmd.Draw(c, 1, Owner); }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
