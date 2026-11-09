using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class VelvetFate() : BlackCatCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{ protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) { await CreatureCmd.Damage(c, Owner.Creature, IsUpgraded ? 3 : 4, ValueProp.Unpowered, Owner.Creature, this); var card = (await CardSelectCmd.FromHandForUpgrade(c, Owner, this)); if (card != null) CardCmd.Upgrade(card); } protected override void OnUpgrade() { } }
