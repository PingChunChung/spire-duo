namespace SpireDuo.SpireDuoCode.Cards;
public sealed class VelvetEclipse() : BlackCatCard(1, CardType.Power, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => IsUpgraded;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VelvetEclipsePower>(1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VelvetEclipsePower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await PowerCmd.Apply<VelvetEclipsePower>(c, Owner.Creature, 1, Owner.Creature, this);
        if (IsUpgraded)
            await CreatureCmd.GainBlock(Owner.Creature, 2m, ValueProp.Move, p);
    }
    protected override void OnUpgrade() { }
}
