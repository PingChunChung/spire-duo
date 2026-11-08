using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;

namespace SpireDuo.SpireDuoCode.Patches;

/// <summary>
/// Our shop sprites are plain Sprite2D scenes that BaseLib converts to NMerchantCharacter on Instantiate. BaseLib only
/// skips the Spine animation calls for nodes built by its factory, not for converted ones, so entering a shop throws,
/// and so does the game-over screen when a run ends in a shop (it plays "die" on every shop character).
/// </summary>
[HarmonyPatch]
public static class MerchantCharacterPatch
{
    [HarmonyPatch(typeof(NMerchantCharacter), "_Ready")]
    [HarmonyPrefix]
    public static bool ReadyPrefix(NMerchantCharacter __instance) => HasSpine(__instance);

    [HarmonyPatch(typeof(NMerchantCharacter), nameof(NMerchantCharacter.PlayAnimation))]
    [HarmonyPrefix]
    public static bool PlayAnimationPrefix(NMerchantCharacter __instance) => HasSpine(__instance);

    private static bool HasSpine(Node node) => node.GetChildCount() > 0 && node.GetChild(0).GetClass() == "SpineSprite";
}
