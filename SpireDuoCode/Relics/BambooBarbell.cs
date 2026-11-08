using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(PandaRelicPool))]
public sealed class BambooBarbell : SpireDuoRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Threshold", 5m), new PowerVar<StrengthPower>(1m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BambooPower>(), HoverTipFactory.FromPower<StrengthPower>()];

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner.Creature)) return;
        if (Owner.Creature.GetPowerAmount<BambooPower>() < DynamicVars["Threshold"].IntValue) return;
        Flash();
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars["StrengthPower"].BaseValue, Owner.Creature, null);
    }
}
