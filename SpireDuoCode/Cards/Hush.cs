namespace SpireDuo.SpireDuoCode.Cards;
public sealed class Hush() : BlackCatCard(1, CardType.Skill, CardRarity.Common, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(1m), new BlockVar(5m, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { foreach (var enemy in CombatState!.HittableEnemies) await PowerCmd.Apply<WeakPower>(c, enemy, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this); if (Owner.Creature.GetPower<FootprintStatePower>()?.Slots[2] == Footprint.Power) await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, p); }
    protected override void OnUpgrade() => DynamicVars["WeakPower"].UpgradeValueBy(1m);
}
