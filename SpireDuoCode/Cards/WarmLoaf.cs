namespace SpireDuo.SpireDuoCode.Cards;

public sealed class WarmLoaf() : BlackCatCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(16m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) => await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(4m);
}
