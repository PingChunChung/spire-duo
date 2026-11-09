using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class EveningWash() : BlackCatCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<EveningWashPower>(12m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<EveningWashPower>()];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) => await PowerCmd.Apply<EveningWashPower>(context, Owner.Creature, DynamicVars["EveningWashPower"].BaseValue, Owner.Creature, this);
    protected override void OnUpgrade() => DynamicVars["EveningWashPower"].UpgradeValueBy(4m);
}
