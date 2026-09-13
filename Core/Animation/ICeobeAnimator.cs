using MegaCrit.Sts2.Core.Bindings.MegaSpine;

namespace Trimaw.Core.Animation;

public interface ICeobeAnimator
{
    Task<CeobeAnimationResult> AnimateAttack(
        MegaAnimationState state,
        CeobeAttack attack,
        int hitCount,
        bool spaceHitsEvenly,
        float timescale);
}