using BaseLib.Abstracts;
using BaseLib.Utils;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Potions;

[Pool(typeof(TrimawPotionPool))]
public abstract class TrimawPotion : CustomPotionModel
{
    public sealed override string CustomPackedImagePath => Icon80Path;
    public sealed override string CustomPackedOutlinePath => IconOutline80Path;
    public sealed override string CustomLargeImagePath => Icon256Path;

    public virtual string Icon80Path => Pathfinder.Potion80(this);

    public virtual string IconOutline80Path => Pathfinder.PotionOutline80(this);

    public virtual string Icon256Path => Pathfinder.Potion256(this);
}