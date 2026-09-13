using Godot;
using MegaCrit.Sts2.Core.Extensions;

namespace Trimaw.Core.Animation;

internal class Timer
{
    private SceneTreeTimer? _timer;

    protected Timer(double time)
    {
        var mainLoop = (SceneTree)Engine.GetMainLoop();
        _timer = mainLoop.CreateTimer(time);
        _timer.Timeout += OnTimeoutInternal;
    }

    public bool TimerRunning => (_timer?.TimeLeft ?? 0) > 0;

    private void OnTimeoutInternal()
    {
        OnTimeout();
        _timer?.Timeout -= OnTimeoutInternal;
        _timer = null;
    }

    protected virtual void OnTimeout()
    {
    }

    public static Timer Start(double time)
    {
        return new Timer(time);
    }

    public async Task WaitForTimeout()
    {
        if (_timer is null) return;
        if (_timer.TimeLeft > 0) await _timer.ToSignal(_timer, SceneTreeTimer.SignalName.Timeout).ToTask();
        _timer = null;
    }
}