namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SoftStep() : BlackCatCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5m, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        // TODO(cat-batch-2): set the right Footprint slot to S.
    }

    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3m);
}
