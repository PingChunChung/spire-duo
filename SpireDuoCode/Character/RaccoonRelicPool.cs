using BaseLib.Abstracts;
using SpireDuo.SpireDuoCode.Extensions;
using Godot;

namespace SpireDuo.SpireDuoCode.Character;

public sealed class RaccoonRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Raccoon.Color;
    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
