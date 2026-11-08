using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;
using SpireDuo.SpireDuoCode.Character;
using SpireDuo.SpireDuoCode.Extensions;

namespace SpireDuo.SpireDuoCode.Events;

/// <summary>
/// Our events only show up in runs with Koala or Panda. BaseLib reads <see cref="CustomEventModel.Acts"/> in the
/// constructor, before the acts are safe to look up, so every event stays a shared one and is limited by act index here.
/// </summary>
public abstract class SpireDuoEvent : CustomEventModel
{
    /// <summary>0-based acts the event can appear in; empty means every act.</summary>
    protected virtual int[] ActIndices => [];

    protected virtual bool RequiresMultiplayer => false;

    /// <summary>Options only run once the event belongs to a player.</summary>
    protected new Player Owner => base.Owner!;

    public override string? CustomInitialPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".EventImagePath();

    public override bool IsAllowed(IRunState runState)
    {
        if (RequiresMultiplayer && runState.Players.Count < 2) return false;
        if (ActIndices.Length > 0 && !ActIndices.Contains(runState.CurrentActIndex)) return false;
        return runState.Players.Any(p => p.Character is Koala or Panda);
    }
}
