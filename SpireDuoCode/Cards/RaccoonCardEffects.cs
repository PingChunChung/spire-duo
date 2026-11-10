using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

internal static class RaccoonCardEffects
{
    internal static Task < int> Stash(PlayerChoiceContext c, CardModel card, int amount = 1, Func<CardModel, bool>? filter = null) => RaccoonStashStatePower.StashFromHand(c, card.Owner, amount, card, filter);
    internal static async Task Hit(PlayerChoiceContext c, CardPlay p, decimal damage)
    {
        if (p.Target != null)
            await DamageCmd.Attack(damage).FromCard(p.Card).Targeting(p.Target).WithHitFx("vfx/vfx_attack_slash").Execute(c);
    }
}
