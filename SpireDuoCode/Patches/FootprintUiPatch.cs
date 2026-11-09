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
    private static readonly Texture2D EmptyTexture = ResourceLoader.Load<Texture2D>(ImageRoot + "footprint_empty.png");
    private static readonly Texture2D AttackTexture = ResourceLoader.Load<Texture2D>(ImageRoot + "footprint_attack.png");
    private static readonly Texture2D SkillTexture = ResourceLoader.Load<Texture2D>(ImageRoot + "footprint_skill.png");
    private static readonly Texture2D PowerTexture = ResourceLoader.Load<Texture2D>(ImageRoot + "footprint_power.png");
    private static readonly Texture2D LockTexture = ResourceLoader.Load<Texture2D>(ImageRoot + "footprint_lock.png");
    private readonly Creature _creature;
    private readonly TextureRect[] _slots = new TextureRect[3];
    private readonly TextureRect[] _locks = new TextureRect[3];

    public NFootprintStrip(Creature creature)
    {
        _creature = creature;
        Name = "NFootprintStrip";
        Size = new Vector2(78, 26);
        MouseFilter = MouseFilterEnum.Stop;
        TooltipText = new LocString("powers", "SPIREDUO-FOOTPRINT_UI_TOOLTIP.description").GetFormattedText();

        for (int i = 0; i < 3; i++)
        {
            var slot = new TextureRect
            {
                CustomMinimumSize = new Vector2(26, 26),
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                MouseFilter = MouseFilterEnum.Ignore
            };
            // Godot resets the size of a plain child Control to its texture, so shrink the 64px lock with Scale instead.
            var locked = new TextureRect
            {
                Texture = LockTexture,
                Position = new Vector2(13, 13),
                Scale = Vector2.One * (14f / LockTexture.GetWidth()),
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

    // Dark backing so empty slots stay readable over the cat's dark fur.
    private static readonly StyleBoxFlat Backing = new()
    {
        BgColor = new Color(0.06f, 0.04f, 0.1f, 0.55f),
        CornerRadiusTopLeft = 8,
        CornerRadiusTopRight = 8,
        CornerRadiusBottomLeft = 8,
        CornerRadiusBottomRight = 8
    };

    public override void _Draw() => DrawStyleBox(Backing, new Rect2(-4, -3, Size.X + 8, Size.Y + 6));

    private void Refresh()
    {
        var state = _creature.GetPower<FootprintStatePower>();
        for (int i = 0; i < 3; i++)
        {
            Footprint footprint = state?.Slots[i] ?? Footprint.Empty;
            Texture2D icon = footprint switch
            {
                Footprint.Attack => AttackTexture,
                Footprint.Skill => SkillTexture,
                Footprint.Power => PowerTexture,
                _ => EmptyTexture
            };
            _slots[i].Texture = icon;
            _locks[i].Visible = state?.Locked[i] ?? false;
        }
    }
}
