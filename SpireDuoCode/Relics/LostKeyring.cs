using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;
[Pool(typeof(RaccoonRelicPool))]
public sealed class LostKeyring : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    public override async Task BeforeCombatStart()
    {
        if (PileType.Deck.GetPile(Owner).Cards.Count(card => card.Type == CardType.Skill) < 8)
            return;
        Flash();
        await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1, Owner.Creature, null);
    }
}
