namespace Trimaw.Core.Animation;

public interface IActionAnimator
{
    public bool AnyAnimationsHaveStarted { get; }
    public bool AllAnimationsHaveFinished { get; }
    public Task WaitForActionStart();
    public Task WaitForActionImpact();
    public Task WaitForActionEnd();
    public Task FinishAllAnimations();
}