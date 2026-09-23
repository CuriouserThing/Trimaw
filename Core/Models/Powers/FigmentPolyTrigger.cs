using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers;

/// <summary>
///     <see cref="FigmentTrigger" /> that can trigger a move an arbitrary number of times, but never on removal.
/// </summary>
public abstract class FigmentPolyTrigger : FigmentTrigger
{
    /// <summary>
    ///     No big icon on <see cref="FigmentPolyTrigger" /> to prevent certain visual quirks related to this power flashing
    ///     while the figment moves.
    /// </summary>
    public sealed override string Icon256Path => Pathfinder.Image("powers", "big", "blank");

    protected sealed override bool ShouldRemoveBeforeTrigger => false;

    protected sealed override bool ShouldTrigger(bool hasBeenRemoved)
    {
        return !hasBeenRemoved;
    }
}