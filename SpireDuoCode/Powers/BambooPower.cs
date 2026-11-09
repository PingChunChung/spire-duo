using SpireDuo.SpireDuoCode.Relics;
using SpireDuo.SpireDuoCode.Enchantments;

namespace SpireDuo.SpireDuoCode.Powers;

/// <summary>Panda's resource. Cards with "Munch N" spend N Bamboo for a bonus effect.</summary>
public sealed class BambooPower : SpireDuoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power != this || amount <= 0 || Owner.Player == null) return;
        await PassingNote.OnResourceGained(choiceContext, Owner.Player);
    }

    /// <summary>Spends <paramref name="cost"/> Bamboo if the creature has that much. Returns whether it did.</summary>
    public static async Task<bool> Munch(PlayerChoiceContext choiceContext, Creature creature, int cost, CardModel? source)
    {
        var crunchyShoot = creature.GetPower<CrunchyShootPower>();
        if (crunchyShoot != null)
        {
            cost = Math.Max(0, cost - crunchyShoot.Amount);
            await PowerCmd.Remove<CrunchyShootPower>(creature);
        }
        var buffet = creature.GetPower<BottomlessBuffetPower>();
        if (buffet != null && buffet.TryDiscountMunch()) cost = Math.Max(1, cost - 1);
        var secondStomach = creature.GetPower<SecondStomachPower>();
        if (secondStomach != null && secondStomach.TryUseFreeMunch())
        {
            await AfterMunch(choiceContext, creature, source, cost);
            return true;
        }
        var bamboo = creature.GetPower<BambooPower>();
        if (bamboo == null || bamboo.Amount < cost)
        {
            var chopsticks = creature.Player?.GetRelic<Chopsticks>();
            if (chopsticks != null) await chopsticks.OnMunchFailed(choiceContext);
            return false;
        }
        await PowerCmd.ModifyAmount(choiceContext, bamboo, -cost, creature, source);
        await AfterMunch(choiceContext, creature, source, cost);
        return true;
    }

    /// <summary>Spends all Bamboo and returns how much was eaten.</summary>
    public static async Task<int> MunchAll(PlayerChoiceContext choiceContext, Creature creature, CardModel? source)
    {
        int amount = creature.GetPowerAmount<BambooPower>();
        if (amount <= 0) return 0;
        await PowerCmd.Remove<BambooPower>(creature);
        await AfterMunch(choiceContext, creature, source, amount);
        return amount;
    }

    private static async Task AfterMunch(PlayerChoiceContext choiceContext, Creature creature, CardModel? source, int amount)
    {
        if (source?.Enchantment is FreshlyCut && creature.GetPower<FreshlyCutPower>() == null)
            await PowerCmd.Apply<FreshlyCutPower>(choiceContext, creature, 4m, creature, source);
        if (creature.GetPower<FullPower>() == null)
            await PowerCmd.Apply<FullPower>(choiceContext, creature, 1m, creature, source);
        if (creature.GetPower<MunchMemoryPower>() == null)
            await PowerCmd.Apply<MunchMemoryPower>(choiceContext, creature, 1m, creature, source);
        var buffet = creature.GetPower<BottomlessBuffetPower>();
        if (buffet != null && amount >= 3)
            await PowerCmd.Apply<StrengthPower>(choiceContext, creature, buffet.Amount, creature, source);
        var ironHide = creature.GetPower<IronHidePower>();
        if (ironHide != null && ironHide.TryTriggerThisTurn())
        {
            ironHide.FlashIcon();
            await CreatureCmd.GainBlock(creature, ironHide.Amount, ValueProp.Unpowered, null);
        }
        var appetite = creature.GetPower<BigAppetitePower>();
        if (appetite != null && appetite.TryTriggerThisTurn())
        {
            appetite.FlashIcon();
            await PowerCmd.Apply<StrengthPower>(choiceContext, creature, appetite.Amount, creature, source);
        }
        var digIn = creature.GetPower<DigInPower>();
        if (digIn != null && creature.Player != null && digIn.TryTriggerThisTurn())
        {
            digIn.FlashIcon();
            await CardPileCmd.Draw(choiceContext, digIn.Amount, creature.Player);
        }
        var addict = creature.GetPower<SnackAddictPower>();
        if (addict != null) await addict.OnMunch(choiceContext);
        var mat = creature.Player?.GetRelic<PicnicMat>();
        if (mat != null) await mat.OnMunch();
        var feast = creature.GetPower<RollingFeastPower>();
        if (feast != null) await RollPower.Gain(choiceContext, creature, feast.Amount, source);
        var sled = creature.Player?.GetRelic<BambooSled>();
        if (sled != null) await sled.OnMunch(choiceContext, creature, source);
    }
}
