namespace SpireDuo.SpireDuoCode.Cards;

public sealed class BristleAndBrush() : BlackCatCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(10m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) { await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play); await FootprintStatePower.Lock(context, Owner.Creature, 1, this); }
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3m);
}
