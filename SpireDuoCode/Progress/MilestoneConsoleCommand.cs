using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Progress;

/// <summary>Developer convenience command discovered automatically by the game's mod console registry.</summary>
public class MilestoneConsoleCommand : AbstractConsoleCmd
{
    public override string CmdName => "sanackoala_milestone";
    public override string Args => "<achievement id|reset|unseen>";
    public override string Description => "Unlock a SanaCkoala Memory Album achievement, reset progress, or replay its reveals.";
    public override bool IsNetworked => false;

    public override CmdResult Process(Player? issuingPlayer, string[] args)
    {
        if (args.Length != 1)
            return new CmdResult(false, "Usage: sanackoala_milestone <achievement id|reset|unseen>");
        if (args[0].Equals("reset", StringComparison.OrdinalIgnoreCase))
        {
            ProgressStore.ResetMilestones();
            return new CmdResult(true, "SanaCkoala Memory Album achievements and stats reset.");
        }
        if (args[0].Equals("unseen", StringComparison.OrdinalIgnoreCase))
        {
            ProgressStore.Current.SeenUnlocks.Clear();
            ProgressStore.Flush();
            return new CmdResult(true, "SanaCkoala Memory Album unlock reveals reset.");
        }

        string id = args[0].ToUpperInvariant();
        if (!MilestoneIds.All.Contains(id))
            return new CmdResult(false, $"Unknown SanaCkoala achievement: {args[0]}");
        MilestoneTracker.Unlock(id);
        return new CmdResult(true, $"SanaCkoala achievement unlocked: {id}");
    }

    public override CompletionResult GetArgumentCompletions(Player? player, string[] args) =>
        CompleteArgument(MilestoneIds.All.Append("reset").Append("unseen"), [], args.FirstOrDefault() ?? string.Empty);
}

public sealed class AchievementConsoleCommand : MilestoneConsoleCommand
{
    public override string CmdName => "sanackoala_achievement";
}
