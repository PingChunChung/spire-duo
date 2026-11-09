using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class NeedleLeap() : BlackCatCard(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{ public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust]; protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4m, ValueProp.Move), new RepeatVar(3)]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { if (p.Target != null) await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(3).FromCard(this).Targeting(p.Target).Execute(c); } protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(1m); }
