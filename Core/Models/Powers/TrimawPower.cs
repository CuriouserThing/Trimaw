using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers;

public abstract class TrimawPower : CustomPowerModel
{
    public sealed override string CustomPackedIconPath => Icon64Path;
    public sealed override string CustomBigIconPath => Icon256Path;

    public virtual string Icon64Path => Pathfinder.Power64(this);

    public virtual string Icon256Path => Pathfinder.Power256(this);

    public abstract override PowerType Type { get; }

    public abstract override PowerStackType StackType { get; }
}