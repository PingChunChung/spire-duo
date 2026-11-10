using BaseLib.Abstracts;
using SpireDuo.SpireDuoCode.Extensions;
using Godot;

namespace SpireDuo.SpireDuoCode.Character;

public sealed class RaccoonCardPool : CustomCardPoolModel
{
    public override string Title => Raccoon.CharacterId;
    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    public override float H => 0.07f;
    public override float S => 0.73f;
    public override float V => 0.79f;
    public override Color DeckEntryCardColor => Raccoon.Color;
    public override bool IsColorless => false;
}
