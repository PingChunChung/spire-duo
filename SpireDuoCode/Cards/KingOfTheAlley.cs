using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class KingOfTheAlley() : RaccoonCard(2, CardType.Power, CardRarity.Ancient, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(18m, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<KingOfTheAlleyPower>()];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        await PowerCmd.Apply<KingOfTheAlleyPower>(c, Owner.Creature, 1m, Owner.Creature, this);
        if (IsUpgraded)
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.BaseValue, ValueProp.Move, p);
    }
    protected override void OnUpgrade()
    {
    }
}
