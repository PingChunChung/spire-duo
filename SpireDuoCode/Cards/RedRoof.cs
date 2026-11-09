using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class RedRoof() : BlackCatCard(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{ protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12m, ValueProp.Move)]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { foreach (var e in Owner.Creature.CombatState!.HittableEnemies) { await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(e).Execute(c); await Owner.Creature.GetPower<FootprintStatePower>()!.RotateRight(c, this); } } protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m); }
