using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class FelineVelocity() : BlackCatCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{ protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FelineVelocityPower>()]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<FelineVelocityPower>(c, Owner.Creature, IsUpgraded ? 2 : 3, Owner.Creature, this); protected override void OnUpgrade() { } }
