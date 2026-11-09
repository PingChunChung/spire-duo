using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class GrandGrooming() : BlackCatCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{ public override bool GainsBlock => true; protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(18m, ValueProp.Move)]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, p); var s = await FootprintStatePower.GetOrCreate(c, Owner.Creature, this); for (int i = 0; i < 3; i++) if (s.Slots[i] == Footprint.Empty && !s.Locked[i]) await s.Set(c, i, Footprint.Skill, this); } protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(6m); }
