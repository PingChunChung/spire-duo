using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;
using SpireDuo.SpireDuoCode.Cards;

namespace SpireDuo.SpireDuoCode.Patches;

/// <summary>
/// Archaic Tooth only knows the vanilla starter cards, so Orobas never offers it to Koala or Panda. Add theirs.
/// Dusty Tome reads the same list to keep these cards out of its pick, which leaves it Lazy Sunday or Rolling Thunder.
/// </summary>
[HarmonyPatch(typeof(ArchaicTooth), "TranscendenceUpgrades", MethodType.Getter)]
public static class ArchaicToothPatch
{
    public static void Postfix(Dictionary<ModelId, CardModel> __result)
    {
        __result[ModelDb.Card<Doze>().Id] = ModelDb.Card<DeepSleep>();
        __result[ModelDb.Card<GatherBamboo>().Id] = ModelDb.Card<BambooHarvest>();
        __result[ModelDb.Card<PocketLining>().Id] = ModelDb.Card<MoonlitHoard>();
    }
}
