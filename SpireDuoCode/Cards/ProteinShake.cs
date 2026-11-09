namespace SpireDuo.SpireDuoCode.Cards;
public sealed class ProteinShake() : PandaCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(2m), new DynamicVar("NoMunchBonus", 1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<BambooPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) { decimal amount = DynamicVars["StrengthPower"].BaseValue; if (Owner.Creature.GetPower<MunchMemoryPower>() == null) amount += DynamicVars["NoMunchBonus"].BaseValue; await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, amount, Owner.Creature, this); }
    protected override void OnUpgrade() { DynamicVars["StrengthPower"].UpgradeValueBy(1m); DynamicVars["NoMunchBonus"].UpgradeValueBy(1m); }
}
