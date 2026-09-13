namespace Trimaw.Core.Animation;

public record struct CeobeAnimationResult(
    bool PlayerWasAnimated,
    float? TimeBeforeFirstHit = null,
    float? TimeBetweenHits = null);