namespace Trimaw.Core.Models.Powers;

/// <summary>
///     <see cref="FigmentTrigger" /> that can only trigger a move once, but can optionally trigger the move when the
///     trigger power is removed.
/// </summary>
public abstract class FigmentMonoTrigger : FigmentTrigger
{
    protected virtual bool ShouldTriggerOnRemoval => false;

    protected sealed override bool ShouldRemoveBeforeTrigger => true;

    protected sealed override bool ShouldTrigger(bool hasBeenRemoved)
    {
        return TriggerCount == 0 && (!hasBeenRemoved || ShouldTriggerOnRemoval);
    }
}