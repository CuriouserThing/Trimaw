using BaseLib.Abstracts;
using BaseLib.Utils;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Relics;

[Pool(typeof(TrimawRelicPool))]
public abstract class TrimawRelic : CustomRelicModel
{
    public sealed override string PackedIconPath => Icon85Path;
    protected sealed override string PackedIconOutlinePath => IconOutline85Path;
    protected sealed override string BigIconPath => Icon256Path;

    public virtual string Icon85Path => Pathfinder.Relic85(this);

    public virtual string IconOutline85Path => Pathfinder.RelicOutline85(this);

    public virtual string Icon256Path => Pathfinder.Relic256(this);
}