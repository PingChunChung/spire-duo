using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using SpireDuo.SpireDuoCode.Character;

namespace SpireDuo.SpireDuoCode.Patches;

[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.GetUnlockText))]
public static class BlackCatUnlockTextPatch
{
    [HarmonyPostfix]
    private static void UseBlackCatText(CharacterModel __instance, ref LocString __result)
    {
        if (__instance is BlackCat)
            __result = new LocString("characters", "SPIREDUO-BLACK_CAT.lockedDescription");
    }
}
