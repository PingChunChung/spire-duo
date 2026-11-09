using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class SoftAuthority() : BlackCatCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{ protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<SoftAuthorityPower>()]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<SoftAuthorityPower>(c, Owner.Creature, IsUpgraded ? 2 : 1, Owner.Creature, this); protected override void OnUpgrade() { } }
