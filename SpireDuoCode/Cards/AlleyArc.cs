namespace SpireDuo.SpireDuoCode.Cards;

public sealed class AlleyArc() : BlackCatCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9m, ValueProp.Move), new DynamicVar("PounceBonus", 6m)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(context);
        if (Owner.Creature.GetPower<FootprintStatePower>()?.WillTrigger(FootprintLine.Pounce, Footprint.Attack) == true)
            await DamageCmd.Attack(DynamicVars["PounceBonus"].BaseValue).FromCard(this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(context);
    }
    protected override void OnUpgrade() { DynamicVars.Damage.UpgradeValueBy(2m); DynamicVars["PounceBonus"].UpgradeValueBy(1m); }
}
