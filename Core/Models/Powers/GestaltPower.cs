using MegaCrit.Sts2.Core.Entities.Powers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers;

public class GestaltPower : FigmentLifecyclePower
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override bool PreventsPopping => true;

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("snake_spiral");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("snake_spiral");
}