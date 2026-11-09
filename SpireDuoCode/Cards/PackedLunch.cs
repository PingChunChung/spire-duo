namespace SpireDuo.SpireDuoCode.Cards;
public sealed class PackedLunch() : PandaCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BambooPower>(3m), new DynamicVar("FullBonus", 2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>(), HoverTipFactory.FromPower<FullPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) { decimal amount = DynamicVars["BambooPower"].BaseValue; if (await FullPower.Consume(choiceContext, Owner.Creature, this)) amount += DynamicVars["FullBonus"].BaseValue; await PowerCmd.Apply<BambooPower>(choiceContext, Owner.Creature, amount, Owner.Creature, this); }
    protected override void OnUpgrade() { DynamicVars["BambooPower"].UpgradeValueBy(1m); DynamicVars["FullBonus"].UpgradeValueBy(1m); }
}
