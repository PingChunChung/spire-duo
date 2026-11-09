using BaseLib.Abstracts;
using SpireDuo.SpireDuoCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace SpireDuo.SpireDuoCode.Enchantments;

/// <summary>At end of turn, a retained enchanted card grants one Block; the turn cap is enforced by the tracker power.</summary>
public sealed class WellRested : CustomEnchantmentModel
{
    protected override string CustomIconPath => "well_rested.png".EnchantmentImagePath();

    public override bool CanEnchant(CardModel card) => base.CanEnchant(card) && card.Keywords.Contains(CardKeyword.Retain);

    public override async Task BeforeFlush(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Card.Owner || Card.Pile?.Type != PileType.Hand || !Card.Keywords.Contains(CardKeyword.Retain)) return;
        await WellRestedMemoryPower.TryGain(choiceContext, player.Creature, Card);
    }
}
