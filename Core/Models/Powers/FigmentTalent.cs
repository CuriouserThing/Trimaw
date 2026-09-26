using MegaCrit.Sts2.Core.Commands;
using Trimaw.Core.ImaginationSystem;

namespace Trimaw.Core.Models.Powers;

public abstract class FigmentTalent : FigmentPower
{
    protected virtual bool ShouldRemoveWhenNoMoves => false;

    internal sealed override async Task AfterMovePerformed(FigmentIntent oldIntent, FigmentIntent? newIntent)
    {
        // Delegate to another method if a child needs this hook, but that's a design smell anyway?
        if (newIntent is null && ShouldRemoveWhenNoMoves) await PowerCmd.Remove(this);
    }

    protected internal virtual MoveParams ModifyMoveParams(MoveParams moveParams, bool dryRun)
    {
        return moveParams;
    }

    protected internal virtual Task AfterModifyingMoveParams(MoveParams originalParams, MoveParams newParams)
    {
        return Task.CompletedTask;
    }
}