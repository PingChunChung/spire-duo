namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DeepScript() : KoalaCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DeepScriptPower>(2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain), HoverTipFactory.Static(StaticHoverTip.Block)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) =>
        await PowerCmd.Apply<DeepScriptPower>(choiceContext, Owner.Creature, DynamicVars["DeepScriptPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["DeepScriptPower"].UpgradeValueBy(1m);
}
