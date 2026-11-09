namespace SpireDuo.SpireDuoCode.Cards;

public sealed class FenceVault() : BlackCatCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) { await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play); await (await FootprintStatePower.GetOrCreate(context, Owner.Creature, this)).RotateRight(context, this); }
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3m);
}
