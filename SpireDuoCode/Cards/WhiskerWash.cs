namespace SpireDuo.SpireDuoCode.Cards;
public sealed class WhiskerWash() : BlackCatCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6m, ValueProp.Move), new CardsVar(1)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, p); await CardPileCmd.Draw(c, 1, Owner); }
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(2m);
}
