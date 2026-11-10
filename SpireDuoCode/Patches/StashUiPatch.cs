using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using SpireDuo.SpireDuoCode.Character;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Patches;
[HarmonyPatch(typeof(NCreature), nameof(NCreature._Ready))]
public static class StashUiPatch
{
    [HarmonyPostfix]
    private static void AddStashStrip(NCreature __instance)
    {
        if (__instance.Entity.Player?.Character is not Raccoon || __instance.GetNodeOrNull<NStashStrip>("NStashStrip") != null)
            return;
        var strip = new NStashStrip(__instance.Entity);
        __instance.AddChild(strip);
        var healthBar = __instance.GetNodeOrNull<Control>("%HealthBar");
        strip.Position =(healthBar?.Position ?? Vector2.Zero) + new Vector2(0, - 62);
        strip.ZIndex = 2;
    }
}
/// <summary> Read-only view of the synchronized Stash power; it never owns gameplay state.</summary>
public sealed partial class NStashStrip : HBoxContainer
{
    private const string ImageRoot = "res://SanaCkoala/images/ui/";
    private readonly Creature _creature;
    private readonly TextureRect[] _slots = new TextureRect[3];
    private static readonly Texture2D Empty = ResourceLoader.Load<Texture2D>(ImageRoot + "stash_empty.png");
    private static readonly Texture2D CardBack = ResourceLoader.Load<Texture2D>(ImageRoot + "stash_cardback.png");
    public NStashStrip(Creature creature)
    {
        _creature = creature;
        Name = "NStashStrip";
        CustomMinimumSize = new Vector2(90, 34);
        TooltipText = new LocString("powers", "SPIREDUO-RACCOON_STASH_UI_TOOLTIP.description").GetFormattedText();
        for (int i = 0; i < _slots.Length; i++)
        {
            var slot = new TextureRect
            {
                CustomMinimumSize = new Vector2(30, 34), ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize, StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered, MouseFilter = MouseFilterEnum.Stop
            }
            ;
            AddChild(slot);
            _slots[i] = slot;
        }
    }
    public override void _Process(double delta)
    {
        var state = _creature.GetPower<RaccoonStashStatePower>();
        for (int i = 0; i < _slots.Length; i++)
        {
            CardModel? card = state?.Slots.ElementAtOrDefault(i);
            _slots[i].Texture = card == null? Empty : CardBack;
            _slots[i].TooltipText = card?.Title ?? TooltipText;
        }
    }
}
