using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace SpireDuo.SpireDuoCode.Cards;

/// <summary>A generated Koala token. It belongs to the token pool, never the Koala reward pool.</summary>
[Pool(typeof(TokenCardPool))]
public sealed class Dream() : SpireDuoCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal, CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move), new BlockVar(6m, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    public static async Task AddToHand(PlayerChoiceContext choiceContext, MegaCrit.Sts2.Core.Entities.Players.Player owner, bool upgraded)
    {
        var dream = owner.Creature.CombatState!.CreateCard<Dream>(owner);
        if (upgraded) CardCmd.Upgrade(dream);
        await CardPileCmd.AddGeneratedCardToCombat(dream, PileType.Hand, owner);
    }

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) => Resolve(choiceContext, cardPlay);

    internal async Task Resolve(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var damage = CombatState!.CreateCard<DreamDamageOption>(Owner);
        var block = CombatState.CreateCard<DreamBlockOption>(Owner);
        if (IsUpgraded)
        {
            CardCmd.Upgrade(damage);
            CardCmd.Upgrade(block);
        }
        var choice = await CardSelectCmd.FromChooseACardScreen(choiceContext, [damage, block], Owner, canSkip: false);
        if (choice is DreamDamageOption)
        {
            var enemies = CombatState.HittableEnemies;
            if (enemies.Count > 0)
            {
                var target = Owner.RunState.Rng.CombatTargets.NextItem(enemies)!;
                await CreatureCmd.Damage(choiceContext, target, DynamicVars.Damage.BaseValue, ValueProp.Move, Owner.Creature, this);
            }
        }
        else if (choice is DreamBlockOption)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars.Block.UpgradeValueBy(2m);
    }
}

[Pool(typeof(TokenCardPool))]
public sealed class DreamDamageOption() : SpireDuoCard(0, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move)];
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2m);
}

[Pool(typeof(TokenCardPool))]
public sealed class DreamBlockOption() : SpireDuoCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6m, ValueProp.Move)];
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(2m);
}
