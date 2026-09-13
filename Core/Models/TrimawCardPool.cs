using BaseLib.Abstracts;
using Godot;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models;

public class TrimawCardPool : CustomCardPoolModel
{
    public override string Title => Trimaw.CharacterId;

    public override bool IsColorless => false;

    // Strong red, much like the reds in Unfettered or meat/chilis
    public override float H => 0f / 360f;
    public override float S => 1.2f;
    public override float V => 0.9f;

    // Approximation of above, but leaning a notch toward darker purple to distinguish from Ironclad deck entries
    public override Color DeckEntryCardColor => Color.Color8(0xb0, 0x33, 0x48);

    public override string BigEnergyIconPath => Pathfinder.UiImage("big_energy");
    public override string TextEnergyIconPath => Pathfinder.UiImage("text_energy");
}