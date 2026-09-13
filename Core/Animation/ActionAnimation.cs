namespace Trimaw.Core.Animation;

/// <summary>
///     A one-size-fits-most description of animations for various creature actions.
/// </summary>
public class ActionAnimation
{
    public ActionAnimation(string actionId, double impactKey, string? idleId)
    {
        ActionId = actionId;
        ImpactKey = impactKey;
        IdleId = idleId;
    }

    public ActionAnimation(string actionId, double startKey, double impactKey, double? endKey, string? idleId)
    {
        ActionId = actionId;
        StartKey = startKey;
        ImpactKey = impactKey;
        EndKey = endKey;
        IdleId = idleId;
    }

    /// <summary>
    ///     Forced timescale of the first animation and any other non-looping animations.
    /// </summary>
    public double? Timescale { get; init; } = null;

    /// <summary>
    ///     Animation to play before action animation, if any.
    /// </summary>
    public string? StartId { get; init; } = null;

    /// <summary>
    ///     Offset (in seconds) into the start animation (if <see cref="StartId" /> is defined) or action animation,
    ///     representing the point before which the animation is safe to partially or fully obscure.
    /// </summary>
    /// <remarks>0.0s by default.</remarks>
    public double StartKey { get; init; }

    /// <summary>
    ///     Action animation to play. Will loop if both <see cref="EndId" /> and <see cref="IdleId" /> are null.
    /// </summary>
    public string ActionId { get; }

    /// <summary>
    ///     Offset (in seconds) into the action animation, representing the point at which impact should happen.
    /// </summary>
    public double ImpactKey { get; }

    /// <summary>
    ///     Animation to play after action animation, if any. Will loop if <see cref="IdleId" /> is null.
    /// </summary>
    public string? EndId { get; init; } = null;

    /// <summary>
    ///     Offset (in seconds) into the end animation (if <see cref="EndId" /> is defined) or action animation,
    ///     representing the point after which the animation is safe to partially or fully obscure.
    /// </summary>
    /// <remarks>
    ///     The end of the animation by default. Can be longer than the duration of the animation to artificially extend
    ///     it.
    /// </remarks>
    public double? EndKey { get; init; }

    /// <summary>
    ///     Offset (in seconds) into the end animation (if <see cref="EndId" /> is defined) or action animation,
    ///     representing the point after which the animation is safe to cutoff into a new animation (like the death animation
    ///     if applicable).
    /// </summary>
    /// <remarks>
    ///     The end of the animation by default. Only necessary if the last animation is exceptionally long.
    /// </remarks>
    public double? CutoffTime { get; init; }

    /// <summary>
    ///     Animation to play after action and end animations, if any. Will always loop.
    /// </summary>
    public string? IdleId { get; }
}