using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers;

public sealed class ImaginaryFriendPower : FigmentDefensePower
{
    public override string Icon64Path => Pathfinder.GameIconsDotnet64("ghost_ally");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("ghost_ally");
}