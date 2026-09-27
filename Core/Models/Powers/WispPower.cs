using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers;

public sealed class WispPower : FigmentDefensePower
{
    public override string Icon64Path => Pathfinder.GameIconsDotnet64("curled_leaf");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("curled_leaf");
}