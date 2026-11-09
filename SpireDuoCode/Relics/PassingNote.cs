using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(SharedRelicPool))]
public sealed class PassingNote : SpireDuoRelic
{
    private readonly HashSet<Player> _playersWhoTriggered = [];

    public override RelicRarity Rarity => RelicRarity.Common;

    public override bool IsAllowed(IRunState runState) => runState.Players.Count > 1;

    public override Task BeforeCombatStart()
    {
        _playersWhoTriggered.Clear();
        return Task.CompletedTask;
    }

    internal static async Task OnResourceGained(PlayerChoiceContext choiceContext, Player player)
    {
        foreach (var relic in player.RunState.Players.Select(p => p.GetRelic<PassingNote>()).Where(r => r != null))
        {
            await relic!.OnPlayerResourceGained(choiceContext, player);
        }
    }

    private async Task OnPlayerResourceGained(PlayerChoiceContext choiceContext, Player player)
    {
        if (!_playersWhoTriggered.Add(player)) return;
        var ally = Owner.RunState.Players.Where(p => p != player && p.Creature.IsAlive)
            .OrderBy(p => PileType.Hand.GetPile(p).Cards.Count)
            .ThenBy(p => Owner.RunState.GetPlayerSlotIndex(p))
            .FirstOrDefault();
        if (ally == null) return;
        Flash();
        await CardPileCmd.Draw(choiceContext, 1, ally);
    }
}
