using SpireDuo.SpireDuoCode.Relics;

namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Panda's resource. Cards with "Munch N" spend N Bamboo for a bonus effect.</summary>
public sealed class BambooPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Spends <paramref name="cost"/> Bamboo if the creature has that much. Returns whether it did.</summary>
    public static async Task<bool> Munch(PlayerChoiceContext choiceContext, Creature creature, int cost, CardModel? source)
    {
        var stomach = creature.GetPower<BottomlessStomachPower>();
        if (stomach != null) cost = Math.Max(1, cost - stomach.Amount);
        var bamboo = creature.GetPower<BambooPower>();
        if (bamboo == null || bamboo.Amount < cost)
        {
            var chopsticks = creature.Player?.GetRelic<Chopsticks>();
            if (chopsticks != null) await chopsticks.OnMunchFailed(choiceContext);
            return false;
        }
        await PowerCmd.ModifyAmount(choiceContext, bamboo, -cost, creature, source);
        await AfterMunch(choiceContext, creature, source);
        return true;
    }

    /// <summary>Spends all Bamboo and returns how much was eaten.</summary>
    public static async Task<int> MunchAll(PlayerChoiceContext choiceContext, Creature creature, CardModel? source)
    {
        int amount = creature.GetPowerAmount<BambooPower>();
        if (amount <= 0) return 0;
        await PowerCmd.Remove<BambooPower>(creature);
        await AfterMunch(choiceContext, creature, source);
        return amount;
    }

    private static async Task AfterMunch(PlayerChoiceContext choiceContext, Creature creature, CardModel? source)
    {
        var ironHide = creature.GetPower<IronHidePower>();
        if (ironHide != null)
        {
            ironHide.FlashIcon();
            await CreatureCmd.GainBlock(creature, ironHide.Amount, ValueProp.Unpowered, null);
        }
        var appetite = creature.GetPower<BigAppetitePower>();
        if (appetite != null)
        {
            appetite.FlashIcon();
            await PowerCmd.Apply<StrengthPower>(choiceContext, creature, appetite.Amount, creature, source);
        }
        var digIn = creature.GetPower<DigInPower>();
        if (digIn != null && creature.Player != null)
        {
            digIn.FlashIcon();
            await CardPileCmd.Draw(choiceContext, digIn.Amount, creature.Player);
        }
        var mat = creature.Player?.GetRelic<PicnicMat>();
        if (mat != null) await mat.OnMunch();
    }
}
