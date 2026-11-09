namespace SpireDuo.SpireDuoCode.Cards;
public sealed class WindowLedge() : BlackCatCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(7m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, p); }
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3m);
}
