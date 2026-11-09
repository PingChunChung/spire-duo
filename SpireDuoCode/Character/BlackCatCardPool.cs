using BaseLib.Abstracts;
using SpireDuo.SpireDuoCode.Extensions;
using Godot;

namespace SpireDuo.SpireDuoCode.Character;

public sealed class BlackCatCardPool : CustomCardPoolModel
{
    public override string Title => BlackCat.CharacterId;
    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    public override float H => 0.72f;
    public override float S => 0.38f;
    public override float V => 0.88f;
    public override Color DeckEntryCardColor => BlackCat.Color;
    public override bool IsColorless => false;
}
