using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers;

public sealed class ImaginaryShieldPower : FigmentDefensePower
{
    public override string Icon64Path => Pathfinder.NotoEmoji64("shield");
    public override string Icon256Path => Pathfinder.NotoEmoji256("shield");
}