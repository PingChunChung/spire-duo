using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Localization;
using SpireDuo.SpireDuoCode.Character;
using SpireDuo.SpireDuoCode.Powers;

namespace SpireDuo.SpireDuoCode.Patches;

[HarmonyPatch(typeof(NCreature), nameof(NCreature._Ready))]
public static class FootprintUiPatch
{
    [HarmonyPostfix]
    private static void AddFootprintStrip(NCreature __instance)
    {
        if (__instance.Entity.Player?.Character is not BlackCat || __instance.GetNodeOrNull<NFootprintStrip>("NFootprintStrip") != null)
            return;

        var strip = new NFootprintStrip(__instance.Entity);
        __instance.AddChild(strip);
        var healthBar = __instance.GetNodeOrNull<Control>("%HealthBar");
        strip.Position = (healthBar?.Position ?? Vector2.Zero) + new Vector2(0, -44);
        strip.ZIndex = 2;
    }
}

/// <summary>Runtime UI at NCreature/NFootprintStrip. It only renders the replicated combat state.</summary>
public sealed partial class NFootprintStrip : HBoxContainer
{
    private const string ImageRoot = "res://SanaCkoala/images/charui/footprints/";
    private readonly Creature _creature;
    private readonly TextureRect[] _slots = new TextureRect[3];
    private readonly TextureRect[] _locks = new TextureRect[3];

    public NFootprintStrip(Creature creature)
    {
        _creature = creature;
        Name = "NFootprintStrip";
        Size = new Vector2(78, 26);
        MouseFilter = MouseFilterEnum.Stop;
        TooltipText = new LocString("powers", "SPIREDUO-FOOTPRINT_STATE_POWER.description").GetFormattedText();

        for (int i = 0; i < 3; i++)
        {
            var slot = new TextureRect
            {
                CustomMinimumSize = new Vector2(26, 26),
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                MouseFilter = MouseFilterEnum.Ignore
            };
            var locked = new TextureRect
            {
                Texture = ResourceLoader.Load<Texture2D>(ImageRoot + "footprint_lock.png"),
                Position = new Vector2(16, 16),
                Size = new Vector2(10, 10),
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                MouseFilter = MouseFilterEnum.Ignore,
                Visible = false
            };
            slot.AddChild(locked);
            AddChild(slot);
            _slots[i] = slot;
            _locks[i] = locked;
        }
        Refresh();
    }

    public override void _Process(double delta) => Refresh();

    private void Refresh()
    {
        var state = _creature.GetPower<FootprintStatePower>();
        for (int i = 0; i < 3; i++)
        {
            Footprint footprint = state?.Slots[i] ?? Footprint.Empty;
            string icon = footprint switch
            {
                Footprint.Attack => "footprint_attack.png",
                Footprint.Skill => "footprint_skill.png",
                Footprint.Power => "footprint_power.png",
                _ => "footprint_empty.png"
            };
            _slots[i].Texture = ResourceLoader.Load<Texture2D>(ImageRoot + icon);
            _locks[i].Visible = state?.Locked[i] ?? false;
        }
    }
}
