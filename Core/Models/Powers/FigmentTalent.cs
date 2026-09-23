using Trimaw.Core.ImaginationSystem;

namespace Trimaw.Core.Models.Powers;

public abstract class FigmentTalent : FigmentPower
{
    protected internal virtual MoveParams ModifyMoveParams(MoveParams moveParams)
    {
        return moveParams;
    }

    protected internal virtual Task AfterModifyingMoveParams(MoveParams originalParams, MoveParams newParams)
    {
        return Task.CompletedTask;
    }
}