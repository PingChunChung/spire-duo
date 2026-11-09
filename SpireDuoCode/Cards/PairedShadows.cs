using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class PairedShadows() : BlackCatCard(1, CardType.Skill, CardRarity.Rare, TargetType.AnyAlly)
{ public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly; public override bool GainsBlock => true; protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(12m, ValueProp.Move)]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, p); if (p.Target != null) await CreatureCmd.GainBlock(p.Target, DynamicVars.Block, p); } protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(4m); }
