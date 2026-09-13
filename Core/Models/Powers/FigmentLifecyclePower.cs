namespace Trimaw.Core.Models.Powers;

public abstract class FigmentLifecyclePower : FigmentPower
{
    public virtual bool PreventsPopping => false;
}