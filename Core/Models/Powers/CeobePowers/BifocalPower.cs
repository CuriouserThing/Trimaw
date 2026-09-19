using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace Trimaw.Core.Models.Powers.CeobePowers;

/// <summary>
///     Marker power (see <see cref="AimPower" />).
/// </summary>
public class BifocalPower : TrimawPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<AimPower>()];
}