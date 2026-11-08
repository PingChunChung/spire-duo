using MegaCrit.Sts2.Core.Entities.Players;

namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>At the end of the owner's turn, every enemy takes damage for each card the owner Retained.</summary>
public sealed class GrudgePower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain)];

    public override async Task AfterFlush(PlayerChoiceContext choiceContext, Player player, IReadOnlyCollection<CardModel> flushedCards, IReadOnlyCollection<CardModel> retainedCards)
    {
        if (player != Owner.Player || retainedCards.Count == 0) return;
        var enemies = CombatState.HittableEnemies.ToList();
        if (enemies.Count == 0) return;
        Flash();
        await CreatureCmd.Damage(choiceContext, enemies, retainedCards.Count * Amount, ValueProp.Unpowered, Owner, null);
    }
}
