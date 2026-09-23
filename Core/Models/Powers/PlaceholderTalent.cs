using MegaCrit.Sts2.Core.Entities.Powers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers;

public class PlaceholderTalent : FigmentTalent
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.NotoEmoji64("construction");
    public override string Icon256Path => Pathfinder.NotoEmoji256("construction");
}