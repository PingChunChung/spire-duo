using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Relics;

[Pool(typeof(PandaRelicPool))]
public sealed class BambooSled : SpireDuoRelic
{
    private int _turn = -1;

    public override RelicRarity Rarity => RelicRarity.Uncommon;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public async Task OnMunch(PlayerChoiceContext choiceContext, Creature creature, CardModel? source)
    {
        int turn = creature.Player?.PlayerCombatState?.TurnNumber ?? 0;
        if (_turn == turn || creature.Player == null) return;

        _turn = turn;
        Flash();
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, creature.Player);
    }
}
