using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(PandaRelicPool))]
public sealed class Beanbag : SpireDuoRelic
{
    private bool _usedThisTurn;

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(3m, ValueProp.Unpowered)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner.Creature)) _usedThisTurn = false;
        return Task.CompletedTask;
    }

    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        if (_usedThisTurn || creature != Owner.Creature || cardSource == null || amount <= 0) return;
        _usedThisTurn = true;
        Flash();
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.BaseValue, ValueProp.Unpowered, null);
    }
}
