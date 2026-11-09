namespace SpireDuo.SpireDuoCode.Cards;

public sealed class FortressNap() : KoalaCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FortressNapPower>(2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>(), HoverTipFactory.FromPower<PlatingPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) =>
        await PowerCmd.Apply<FortressNapPower>(choiceContext, Owner.Creature, DynamicVars["FortressNapPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["FortressNapPower"].UpgradeValueBy(1m);
}
