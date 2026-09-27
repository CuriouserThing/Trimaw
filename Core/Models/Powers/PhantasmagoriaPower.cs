using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers;

public class PhantasmagoriaPower : FigmentDefensePower
{
    public override string Icon64Path => Pathfinder.GameIconsDotnet64("snake_spiral");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("snake_spiral");
}