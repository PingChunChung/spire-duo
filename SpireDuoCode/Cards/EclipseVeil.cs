namespace SpireDuo.SpireDuoCode.Cards;

public sealed class EclipseVeil() : BlackCatCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8m, ValueProp.Move), new DynamicVar("PowerBlock", 4m), new CardsVar(1)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        if (Owner.Creature.GetPower<FootprintStatePower>()?.Slots[0] != Footprint.Power)
            return;

        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars["PowerBlock"].BaseValue, ValueProp.Move, play);
        await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Owner);
    }
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3m);
}
