using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Cards;

public sealed class MidnightAuction() : RaccoonCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2), new CardsVar(3), new BlockVar(18m, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [EnergyHoverTip];
    protected override async Task OnPlay(PlayerChoiceContext c, CardPlay p)
    {
        if (await RaccoonStashStatePower.Spend(c, Owner.Creature, this) == null)
            return;
        var energy = CombatState!.CreateCard<MidnightAuctionEnergyOption>(Owner);
        var draw = CombatState.CreateCard<MidnightAuctionDrawOption>(Owner);
        var block = CombatState.CreateCard<MidnightAuctionBlockOption>(Owner);
        if (IsUpgraded)
        {
            CardCmd.Upgrade(energy);
            CardCmd.Upgrade(draw);
            CardCmd.Upgrade(block);
        }
        var choice = await CardSelectCmd.FromChooseACardScreen(c,[energy, draw, block], Owner, canSkip : false);
        if (choice is MidnightAuctionEnergyOption) await PlayerCmd.GainEnergy((int)
            DynamicVars.Energy.BaseValue +(IsUpgraded? 1 : 0), Owner);
        else if (choice is MidnightAuctionDrawOption) await CardPileCmd.Draw(c,(int) DynamicVars.Cards.BaseValue +(IsUpgraded? 1 : 0), Owner);
        else if (choice is MidnightAuctionBlockOption) await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.BaseValue +(IsUpgraded? 3m : 0m), ValueProp.Move, p);
    }
    protected override void OnUpgrade()
    {
    }
}
