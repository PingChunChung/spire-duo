using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class OraclesPaw() : BlackCatCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{ protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { await FootprintStatePower.SetAll(c, Owner.Creature, Footprint.Power, Footprint.Skill, Footprint.Power, this); await (await FootprintStatePower.GetOrCreate(c, Owner.Creature, this)).TriggerOmenNow(c, p); } protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1); }
