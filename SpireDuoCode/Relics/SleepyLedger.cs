using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(KoalaRelicPool))]
public sealed class SleepyLedger : SpireDuoRelic
{
    private int _triggerTurn = -1;
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain), HoverTipFactory.FromPower<DrowsyPower>()];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner || !cardPlay.Card.Keywords.Contains(CardKeyword.Retain)) return;
        int turn = Owner.PlayerCombatState?.TurnNumber ?? -1;
        if (_triggerTurn == turn) return;
        _triggerTurn = turn;
        Flash();
        await PowerCmd.Apply<DrowsyPower>(choiceContext, Owner.Creature, 2, Owner.Creature, cardPlay.Card);
    }
}
