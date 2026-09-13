using MegaCrit.Sts2.Core.Bindings.MegaSpine;

namespace Trimaw.Core.Animation;

public class StandardCeobeAnimator : ICeobeAnimator
{
    // Consistent across all three Ceobe skins!
    private const float StanceSwapDuration = 0.33f;
    private const float StaffImpactKey = 0.70f;
    private const float AxeSpearImpactKey = 1.00f;
    private const float StandardAttackDuration = 1.60f;
    private const float KnifeImpactKey = 0.25f;
    private const float KnifeAttackDuration = 0.63f;
    private const string StaffAttackId = "Attack";
    private const string AxeAttackId = "Skill_1";
    private const string SpearAttackId = "Skill_3";
    private const string KnifeAttackId = "Skill_2_Loop";
    private const string StandardStanceIdleId = "Idle";
    private const string SwapToKnifeStanceId = "Skill_2_Begin";
    private const string KnifeStanceIdleId = "Skill_2_Idle";
    private const string SwapToStandardStanceId = "Skill_2_End";

    private static readonly Dictionary<CeobeAttack, CeobeAttackData> AttackData = new()
    {
        [CeobeAttack.Staff] = new CeobeAttackData(false, StaffAttackId, StaffImpactKey),
        [CeobeAttack.Axe] = new CeobeAttackData(false, AxeAttackId, AxeSpearImpactKey),
        [CeobeAttack.Knife] = new CeobeAttackData(true, KnifeAttackId, KnifeImpactKey),
        [CeobeAttack.Spear] = new CeobeAttackData(false, SpearAttackId, AxeSpearImpactKey)
    };

    private Timer? _animationTimer;
    private Timer? _attackDelayTimer;
    private CeobeAttackData _previousAttack = AttackData[CeobeAttack.Staff];

    public float BaselineStaffAttackTimescale { get; init; } = 1f;

    public float BaselineAxeAttackTimescale { get; init; } = 1f;

    public float BaselineKnifeAttackTimescale { get; init; } = 1f;

    public float BaselineSpearAttackTimescale { get; init; } = 1f;

    public async Task<CeobeAnimationResult> AnimateAttack(
        MegaAnimationState state,
        CeobeAttack attack,
        int hitCount,
        bool spaceHitsEvenly,
        float timescale)
    {
        if (hitCount < 1) return new CeobeAnimationResult(false);

        await WaitForPendingAnimations();
        if (!AttackData.TryGetValue(attack, out var attackData)) attackData = AttackData[CeobeAttack.Staff];

        timescale *= attack switch
        {
            CeobeAttack.Staff => BaselineStaffAttackTimescale,
            CeobeAttack.Axe => BaselineAxeAttackTimescale,
            CeobeAttack.Knife => BaselineKnifeAttackTimescale,
            CeobeAttack.Spear => BaselineSpearAttackTimescale,
            _ => 1f
        };

        return hitCount == 1
            ? AnimateSingleHit(state, attackData, timescale)
            : AnimateMultipleHits(state, attackData, hitCount, spaceHitsEvenly, timescale);
    }

    private CeobeAnimationResult AnimateSingleHit(MegaAnimationState state, CeobeAttackData attackData, float timescale)
    {
        var totalDuration = 0f;
        AnimateFirstHit(state, attackData, timescale, ref totalDuration, out var timeBeforeHit);
        state.AddAnimation(attackData.IdleId, loop: true);
        _animationTimer = Timer.Start(totalDuration);
        _previousAttack = attackData;
        return new CeobeAnimationResult(true, timeBeforeHit);
    }

    private CeobeAnimationResult AnimateMultipleHits(
        MegaAnimationState state,
        CeobeAttackData attackData,
        int hitCount,
        bool spaceHitsEvenly,
        float timescale)
    {
        // A rough idea of the amount of extra time STS2 will spend between hits
        // This is impossible to know for sure, since hook listeners can spend an arbitrary amount of time responding to hits
        // We're just looking for a sensible default as we animate attacks outside the vanilla system
        // TODO: At some point we should try harder to use vanilla attack animation *where we're able to*
        const float hitOffset = 0.12f;

        float timeBeforeFirstHit;
        var timeBetweenHits = attackData.Duration / timescale - hitOffset;
        float totalDuration = 0;

        if (spaceHitsEvenly)
        {
            timeBeforeFirstHit = timeBetweenHits;
            var delay = timeBetweenHits - attackData.ImpactKey / timescale;
            var animationIds = new List<string>(hitCount + 1);
            if (attackData.UsesKnifeStance != _previousAttack.UsesKnifeStance)
            {
                totalDuration += StanceSwapDuration / timescale;
                delay -= StanceSwapDuration / timescale;
                animationIds.Add(attackData.SwapToStanceId);
            }

            totalDuration += hitCount * attackData.Duration / timescale;
            for (var i = 0; i < hitCount; i += 1) animationIds.Add(attackData.AttackId);

            // Logically shouldn't be negative, but just accept some desync if it is
            delay = Math.Max(0, delay);
            totalDuration += delay;

            _attackDelayTimer = CeobeAttackDelayTimer.Start(delay, state, animationIds, timescale, attackData.IdleId);
        }
        else
        {
            AnimateFirstHit(state, attackData, timescale, ref totalDuration, out timeBeforeFirstHit);
            var extraHits = hitCount + 1;
            totalDuration += extraHits * attackData.Duration / timescale;
            for (var i = 0; i < extraHits; i += 1)
            {
                var entry = state.AddAnimationTracked(attackData.AttackId, loop: false);
                entry.SetTimeScale(timescale);
            }
        }

        _animationTimer = Timer.Start(totalDuration);
        _previousAttack = attackData;
        return new CeobeAnimationResult(true, timeBeforeFirstHit, timeBetweenHits);
    }

    private void AnimateFirstHit(
        MegaAnimationState state,
        CeobeAttackData attackData,
        float timescale,
        ref float totalDuration,
        out float timeBeforeHit)
    {
        if (attackData.UsesKnifeStance != _previousAttack.UsesKnifeStance)
        {
            timeBeforeHit = (StanceSwapDuration + attackData.ImpactKey) / timescale;
            totalDuration += (StanceSwapDuration + attackData.Duration) / timescale;

            state.SetAnimation(attackData.SwapToStanceId, false);
            state.GetCurrent(0)?.SetTimeScale(timescale);
            var entry = state.AddAnimationTracked(attackData.AttackId, loop: false);
            entry.SetTimeScale(timescale);
        }
        else
        {
            timeBeforeHit = attackData.ImpactKey / timescale;
            totalDuration += attackData.Duration / timescale;

            state.SetAnimation(attackData.AttackId, false);
            state.GetCurrent(0)?.SetTimeScale(timescale);
        }
    }

    private async Task WaitForPendingAnimations()
    {
        if (_animationTimer is not null)
        {
            await _animationTimer.WaitForTimeout();
            _animationTimer = null;
        }

        if (_attackDelayTimer is not null)
        {
            await _attackDelayTimer.WaitForTimeout();
            _attackDelayTimer = null;
        }
    }

    private readonly struct CeobeAttackData(bool usesKnifeStance, string attackId, float impactKey)
    {
        public bool UsesKnifeStance { get; } = usesKnifeStance;
        public string AttackId { get; } = attackId;
        public float ImpactKey { get; } = impactKey;

        public string IdleId => UsesKnifeStance ? KnifeStanceIdleId : StandardStanceIdleId;
        public string SwapToStanceId => UsesKnifeStance ? SwapToKnifeStanceId : SwapToStandardStanceId;
        public float Duration => UsesKnifeStance ? KnifeAttackDuration : StandardAttackDuration;
    }
}