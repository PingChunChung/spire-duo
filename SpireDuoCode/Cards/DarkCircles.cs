namespace SpireDuo.SpireDuoCode.Cards;

public sealed class DarkCircles() : PandaCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new PowerVar<BambooPower>(2m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [EnergyHoverTip, HoverTipFactory.FromPower<BambooPower>(), HoverTipFactory.FromCard<MegaCrit.Sts2.Core.Models.Cards.Dazed>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        await PowerCmd.Apply<BambooPower>(choiceContext, Owner.Creature, DynamicVars["BambooPower"].BaseValue, Owner.Creature, this);
        CardModel dazed = CombatState!.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Dazed>(Owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(dazed, PileType.Discard, Owner));
    }
    protected override void OnUpgrade()
    {
        DynamicVars["BambooPower"].UpgradeValueBy(1m);
    }
}
