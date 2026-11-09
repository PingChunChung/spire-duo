namespace SpireDuo.SpireDuoCode.Cards;

public abstract class UncommonCatCard(int cost, CardType type, TargetType target) : BlackCatCard(cost, type, CardRarity.Uncommon, target)
{
    public override bool GainsBlock => Type == CardType.Skill && TargetType == TargetType.Self;
    protected override IEnumerable<DynamicVar> CanonicalVars => Type switch
    {
        CardType.Attack => [new DamageVar(8m, ValueProp.Move)],
        CardType.Skill when TargetType == TargetType.Self => [new BlockVar(8m, ValueProp.Move)],
        _ => []
    };
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        if (Type == CardType.Attack && play.Target != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(context);
        else if (Type == CardType.Skill && TargetType == TargetType.Self)
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
    }
    protected override void OnUpgrade()
    {
        if (Type == CardType.Attack) DynamicVars.Damage.UpgradeValueBy(2m);
        else if (Type == CardType.Skill && TargetType == TargetType.Self) DynamicVars.Block.UpgradeValueBy(2m);
    }
}

public sealed class OmenReader() : UncommonCatCard(2, CardType.Power, TargetType.Self);
public sealed class HalfMoonRitual() : UncommonCatCard(1, CardType.Skill, TargetType.Self);
public sealed class InkedWindow() : UncommonCatCard(1, CardType.Power, TargetType.Self);
public sealed class Foretell() : UncommonCatCard(1, CardType.Skill, TargetType.Self);
public sealed class LanternShadow() : UncommonCatCard(2, CardType.Power, TargetType.Self);
public sealed class NineLives() : UncommonCatCard(0, CardType.Skill, TargetType.Self);
public sealed class MidnightOracle() : UncommonCatCard(2, CardType.Power, TargetType.Self);
public sealed class EclipseVeil() : UncommonCatCard(1, CardType.Skill, TargetType.Self);
public sealed class FamiliarRoute() : UncommonCatCard(1, CardType.Skill, TargetType.Self);
public sealed class ThreadTheNeedle() : UncommonCatCard(1, CardType.Attack, TargetType.AnyEnemy);
public sealed class MoonbeamMessage() : UncommonCatCard(1, CardType.Skill, TargetType.AnyAlly);
public sealed class TandemProwl() : UncommonCatCard(1, CardType.Power, TargetType.Self);
public sealed class WatchTheirBack() : UncommonCatCard(1, CardType.Skill, TargetType.AnyAlly);
public sealed class BorrowedRhythm() : UncommonCatCard(0, CardType.Skill, TargetType.AnyAlly);
public sealed class QuietCompany() : UncommonCatCard(2, CardType.Power, TargetType.Self);
public sealed class PassingBlink() : UncommonCatCard(1, CardType.Attack, TargetType.AnyEnemy);
public sealed class TwinMoon() : UncommonCatCard(1, CardType.Skill, TargetType.AnyAlly);
public sealed class LanternPass() : UncommonCatCard(1, CardType.Skill, TargetType.AnyAlly);
