namespace SpireDuo.SpireDuoCode.Cards;

public sealed class ThreadTheNeedle() : BlackCatCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move), new DynamicVar("OmenDamage", 18m)];
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        var state = await FootprintStatePower.GetOrCreate(context, Owner.Creature, this);
        decimal damage = state.Slots[0] == Footprint.Power && state.Slots[1] == Footprint.Skill ? DynamicVars["OmenDamage"].BaseValue : DynamicVars.Damage.BaseValue;
        await DamageCmd.Attack(damage).FromCard(this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(context);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["OmenDamage"].UpgradeValueBy(4m);
    }
}
