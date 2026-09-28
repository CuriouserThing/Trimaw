using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.CeobePowers;

/// <summary>
///     Marker power (see <see cref="AimPower" />).
/// </summary>
public class BifocalPower : TrimawPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string Icon64Path => Pathfinder.NotoEmoji64("eyeglasses");
    public override string Icon256Path => Pathfinder.NotoEmoji256("eyeglasses");

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<AimPower>()];
}