namespace SpireDuo.SpireDuoCode.Cards;

public sealed class ThousandPaws() : PandaCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2m, ValueProp.Move), new RepeatVar(8), new DynamicVar("BambooPerHit", 1m), new DynamicVar("MaxBonusHits", 5m)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        int bonusHits = Math.Min(Owner.Creature.GetPowerAmount<BambooPower>() / DynamicVars["BambooPerHit"].IntValue, DynamicVars["MaxBonusHits"].IntValue);
        int hits = DynamicVars.Repeat.IntValue + bonusHits;
        if (bonusHits > 0) await BambooPower.Munch(choiceContext, Owner.Creature, bonusHits * DynamicVars["BambooPerHit"].IntValue, this);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(hits).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(2m);
    }
}
