using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class CleansingSprint() : BlackCatCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{ protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14m, ValueProp.Move), new DynamicVar("PerExhaust", 2m)]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { if (p.Target != null) await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(p.Target).Execute(c); } protected override void OnUpgrade() { DynamicVars.Damage.UpgradeValueBy(4m); DynamicVars["PerExhaust"].UpgradeValueBy(1m); } }
