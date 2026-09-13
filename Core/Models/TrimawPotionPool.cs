using BaseLib.Abstracts;
using Godot;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models;

public class TrimawPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => Trimaw.LabOutlineColor;

    public override string BigEnergyIconPath => Pathfinder.UiImage("big_energy");
    public override string TextEnergyIconPath => Pathfinder.UiImage("text_energy");
}