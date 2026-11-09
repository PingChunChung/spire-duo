namespace SpireDuo.SpireDuoCode.Cards;

public sealed class PassingBlink() : BlackCatCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move), new DynamicVar("CompanionDamage", 15m)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        var state = await FootprintStatePower.GetOrCreate(context, Owner.Creature, this);
        decimal damage = state.CompanionArmed ? DynamicVars["CompanionDamage"].BaseValue : DynamicVars.Damage.BaseValue;
        await DamageCmd.Attack(damage).FromCard(this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(context);
        if (state.CompanionArmed)
            await state.ResolveCompanionNow(context, play);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["CompanionDamage"].UpgradeValueBy(3m);
    }
}
