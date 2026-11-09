using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;
using SpireDuo.SpireDuoCode.Cards;

namespace SpireDuo.SpireDuoCode.Relics;

/// <summary>First wake each combat creates a free Dream. Triggered from <see cref="DrowsyPower.WakeUp"/>.</summary>
[Pool(typeof(KoalaRelicPool))]
public sealed class DreamCatcherBell : SpireDuoRelic
{
    private bool _used;
    public override RelicRarity Rarity => RelicRarity.Shop;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];
    public override Task BeforeCombatStart()
    {
        _used = false;
        return Task.CompletedTask;
    }
    internal async Task OnWake(PlayerChoiceContext choiceContext)
    {
        if (_used) return;
        _used = true;
        Flash();
        var dream = Owner.Creature.CombatState!.CreateCard<Dream>(Owner);
        dream.SetToFreeThisTurn();
        await CardPileCmd.AddGeneratedCardToCombat(dream, PileType.Hand, Owner);
    }
}
