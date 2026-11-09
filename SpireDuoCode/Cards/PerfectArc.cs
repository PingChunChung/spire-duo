using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class PerfectArc() : BlackCatCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{ protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10m, ValueProp.Move)]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { if (p.Target == null) return; await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(p.Target).Execute(c); var s = Owner.Creature.GetPower<FootprintStatePower>(); if (s?.HasTriggered(FootprintLine.Pounce) == true) await DamageCmd.Attack(8).FromCard(this).Targeting(p.Target).Execute(c); } protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m); }
