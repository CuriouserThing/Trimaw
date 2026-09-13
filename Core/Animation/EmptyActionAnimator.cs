namespace Trimaw.Core.Animation;

public class EmptyActionAnimator : IActionAnimator
{
    private EmptyActionAnimator()
    {
    }

    public static IActionAnimator Instance { get; } = new EmptyActionAnimator();

    public bool AnyAnimationsHaveStarted => true;
    public bool AllAnimationsHaveFinished => true;

    public Task WaitForActionStart()
    {
        return Task.CompletedTask;
    }

    public Task WaitForActionImpact()
    {
        return Task.CompletedTask;
    }

    public Task WaitForActionEnd()
    {
        return Task.CompletedTask;
    }

    public Task FinishAllAnimations()
    {
        return Task.CompletedTask;
    }
}