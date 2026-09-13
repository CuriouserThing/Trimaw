using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace Trimaw.Core.Animation;

public class StandardActionAnimator : IActionAnimator
{
    private readonly ActionAnimation _animation;
    private readonly Creature _creature;
    private readonly Timer _startTimer;
    private Timer? _allTimer;
    private Timer? _endTimer;
    private Timer? _impactTimer;

    private StandardActionAnimator(Creature creature, ActionAnimation animation, float inboundTweenLength)
    {
        _creature = creature;
        _animation = animation;
        var timeToStart = Math.Max(0, inboundTweenLength - Scale(_animation.StartKey));
        _startTimer = Timer.Start(timeToStart);
    }

    public bool AnyAnimationsHaveStarted { get; private set; }

    public bool AllAnimationsHaveFinished =>
        AnyAnimationsHaveStarted && _allTimer is not null && !_allTimer.TimerRunning;

    public async Task WaitForActionStart()
    {
        if (AnyAnimationsHaveStarted) return;
        AnyAnimationsHaveStarted = true;
        await _startTimer.WaitForTimeout();
        if (_creature.GetCreatureNode()?.Visuals.SpineBody?.TryGetAnimationState() is { } state)
        {
            Start(state);
        }
        else
        {
            MainFile.Logger.Warn(
                $"Could not get animation state for creature {_creature}. Action animation not possible");
            _impactTimer = Timer.Start(0.25f);
            _endTimer = Timer.Start(0.50f);
            _allTimer = Timer.Start(0.50f);
        }
    }

    public async Task WaitForActionImpact()
    {
        if (!AnyAnimationsHaveStarted) await WaitForActionStart();
        if (_impactTimer is not null) await _impactTimer.WaitForTimeout();
    }

    public async Task WaitForActionEnd()
    {
        if (!AnyAnimationsHaveStarted) await WaitForActionStart();
        if (_endTimer is not null) await _endTimer.WaitForTimeout();
    }

    public async Task FinishAllAnimations()
    {
        if (!AnyAnimationsHaveStarted) await WaitForActionStart();
        if (_allTimer is not null) await _allTimer.WaitForTimeout();
    }

    public static IActionAnimator Begin(Creature creature, ActionAnimation animation, float inboundTweenLength)
    {
        return new StandardActionAnimator(creature, animation, inboundTweenLength);
    }

    private void Start(MegaAnimationState state)
    {
        float startAnimDuration, actionAnimDuration;
        var actionId = _animation.ActionId;
        var loopAction = _animation.EndId is null && _animation.IdleId is null;
        if (_animation.StartId is { } startId)
        {
            state.SetAnimation(startId, false);
            var start = state.GetCurrent(0);
            startAnimDuration = start?.GetAnimationDuration() ?? 0;
            Scale(start, ref startAnimDuration);

            var action = state.AddAnimationTracked(actionId, 0, loopAction);
            actionAnimDuration = action.GetAnimationDuration();
            if (!loopAction) Scale(action, ref actionAnimDuration);
        }
        else
        {
            startAnimDuration = 0;

            state.SetAnimation(actionId, loopAction);
            var action = state.GetCurrent(0);
            actionAnimDuration = action?.GetAnimationDuration() ?? 0;
            Scale(action, ref actionAnimDuration);
        }

        var timeToImpact = startAnimDuration + Scale(_animation.ImpactKey);
        var timeToEnd = startAnimDuration; // ...plus more
        var timeToCutoff = startAnimDuration; // ...plus more

        float lastAnimDuration;
        if (_animation.EndId is { } endId)
        {
            var loopEnd = _animation.IdleId is null;
            var end = state.AddAnimationTracked(endId, 0, loopEnd);
            lastAnimDuration = end.GetAnimationDuration();
            if (!loopEnd) Scale(end, ref lastAnimDuration);

            timeToEnd += actionAnimDuration;
            timeToCutoff += actionAnimDuration;
        }
        else
        {
            lastAnimDuration = actionAnimDuration;
        }

        if (_animation.EndKey is { } endKey)
            timeToEnd += Scale(endKey);
        else
            timeToEnd += lastAnimDuration;

        if (_animation.CutoffTime is { } cutoffTime)
            timeToCutoff += Scale(cutoffTime);
        else
            timeToCutoff += lastAnimDuration;

        if (_animation.IdleId is { } idleId) state.AddAnimation(idleId);

        var timeToVeryEnd = Math.Max(timeToCutoff, Math.Max(timeToImpact, timeToEnd));

        if (timeToImpact > 0) _impactTimer = Timer.Start(timeToImpact);
        if (timeToEnd > 0) _endTimer = Timer.Start(timeToEnd);
        if (timeToVeryEnd > 0) _allTimer = Timer.Start(timeToVeryEnd);
    }

    private void Scale(MegaTrackEntry? entry, ref float duration)
    {
        if (_animation.Timescale is not { } timescale ||
            entry is null) return;

        entry.SetTimeScale((float)timescale);
        duration /= (float)timescale;
    }

    private float Scale(double duration)
    {
        return (float)(duration / (_animation.Timescale ?? 1.0));
    }
}