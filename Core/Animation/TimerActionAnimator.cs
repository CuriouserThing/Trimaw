namespace Trimaw.Core.Animation;

public class TimerActionAnimator : IActionAnimator
{
    private readonly Timer _start, _impact, _end;

    private TimerActionAnimator(double startKey, double impactKey, double endKey)
    {
        _start = Timer.Start(startKey);
        _impact = Timer.Start(impactKey);
        _end = Timer.Start(endKey);
    }

    public bool AnyAnimationsHaveStarted => true;
    public bool AllAnimationsHaveFinished => !_start.TimerRunning && !_impact.TimerRunning && !_end.TimerRunning;

    public async Task WaitForActionStart()
    {
        await _start.WaitForTimeout();
    }

    public async Task WaitForActionImpact()
    {
        await WaitForActionStart();
        await _impact.WaitForTimeout();
    }

    public async Task WaitForActionEnd()
    {
        await WaitForActionImpact();
        await _end.WaitForTimeout();
    }

    public async Task FinishAllAnimations()
    {
        await WaitForActionEnd();
    }

    public static IActionAnimator Begin(double startKey, double impactKey, double endKey)
    {
        return new TimerActionAnimator(startKey, impactKey, endKey);
    }
}