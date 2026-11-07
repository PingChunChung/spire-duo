using BaseLib.Abstracts;
using SpireDuo.SpireDuoCode.Extensions;
using Godot;

namespace SpireDuo.SpireDuoCode.Character;

public class PandaPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => Panda.Color;
    

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}