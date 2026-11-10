using MegaCrit.Sts2.Core.CardSelection;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class AlleyShield() : RaccoonCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6m, ValueProp.Move), new DynamicVar("Bonus", 4m)];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, p);
        int skills = Owner.Creature.GetPower<RaccoonStashStatePower>()?.Slots.Count(x => x.Type == CardType.Skill) ?? 0;
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars["Bonus"].BaseValue * skills, ValueProp.Move, p);
    }
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3m);
}
