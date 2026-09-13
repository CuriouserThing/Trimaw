using MegaCrit.Sts2.Core.Bindings.MegaSpine;

namespace Trimaw.Core.Animation;

internal sealed class CeobeAttackDelayTimer : Timer
{
    private IReadOnlyList<string>? _animationIds;
    private string? _idleId;
    private MegaAnimationState? _state;
    private float _timescale = 1f;

    private CeobeAttackDelayTimer(double time) : base(time)
    {
    }

    public static Timer Start(double delay, MegaAnimationState state, IReadOnlyList<string> animationIds,
        float timescale, string idleId)
    {
        return new CeobeAttackDelayTimer(delay)
        {
            _state = state,
            _animationIds = animationIds,
            _timescale = timescale,
            _idleId = idleId
        };
    }

    protected override void OnTimeout()
    {
        if (_state is null || _animationIds?.Count is null or 0) return;

        _state.SetAnimation(_animationIds[0], false);
        _state.GetCurrent(0)?.SetTimeScale(_timescale);
        foreach (var animation in _animationIds.Skip(1))
        {
            var entry = _state.AddAnimationTracked(animation, loop: false);
            entry.SetTimeScale(_timescale);
        }

        if (_idleId is not null) _state.AddAnimation(_idleId, loop: true);

        _state = null;
        _animationIds = null;
    }
}