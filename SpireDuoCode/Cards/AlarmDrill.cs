namespace SpireDuo.SpireDuoCode.Cards;

public sealed class AlarmDrill() : KoalaCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move), new DynamicVar("Per", 4m), new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DrowsyPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt").Execute(choiceContext);
        int removed = await DrowsyPower.WakeUp(choiceContext, Owner.Creature);
        if (removed > 0)
            await CardPileCmd.Draw(choiceContext, removed / DynamicVars["Per"].IntValue * DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["Per"].UpgradeValueBy(-1m);
    }
}
