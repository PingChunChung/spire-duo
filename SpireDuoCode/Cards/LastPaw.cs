namespace SpireDuo.SpireDuoCode.Cards;

public sealed class LastPaw() : BlackCatCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) { ArgumentNullException.ThrowIfNull(play.Target); var state = await FootprintStatePower.GetOrCreate(context, Owner.Creature, this); int hits = state.PlayedCardsThisTurn == 2 ? 2 : 1; await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(hits).FromCard(this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(context); }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2m);
}
