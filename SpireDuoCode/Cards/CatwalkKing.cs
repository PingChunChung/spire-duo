using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class CatwalkKing() : BlackCatCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{ protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<CatwalkKingPower>()]; protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p) => await PowerCmd.Apply<CatwalkKingPower>(c, Owner.Creature, 1, Owner.Creature, this); protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1); }
