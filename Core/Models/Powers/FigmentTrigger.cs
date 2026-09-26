using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.ImaginationSystem;

namespace Trimaw.Core.Models.Powers;

public abstract class FigmentTrigger : FigmentPower
{
    protected int TriggerCount { get; private set; }

    protected abstract bool ShouldRemoveBeforeTrigger { get; }

    protected abstract bool ShouldTrigger(bool hasBeenRemoved);

    /// <summary>
    ///     Removes self, then informs the figment to use its intended move.
    /// </summary>
    /// <param name="choiceContext">
    ///     Any <see cref="PlayerChoiceContext" /> available to the caller. If null, this method will create
    ///     its own <see cref="BlockingPlayerChoiceContext" /> or <see cref="HookPlayerChoiceContext" /> as appropriate.
    /// </param>
    /// <param name="moveParams">Optional params to pass to the intent.</param>
    protected async Task TriggerMove(PlayerChoiceContext? choiceContext, MoveParams? moveParams = null)
    {
        if (!ShouldTrigger(false)) return;
        if (ShouldRemoveBeforeTrigger && Figment is { IsGone: false }) await PowerCmd.Remove(this);
        await TriggerInternal(choiceContext, moveParams);
    }

    protected sealed override async Task AfterRemoved()
    {
        if (ShouldTrigger(true)) await TriggerInternal(null, null);
    }

    private async Task TriggerInternal(PlayerChoiceContext? choiceContext, MoveParams? moveParams)
    {
        TriggerCount += 1;
        var player = Owner.PetOwner;
        if (player is null) return;

        if (CombatState.CurrentSide == CombatSide.Enemy || !LocalContext.NetId.HasValue)
            choiceContext ??= new BlockingPlayerChoiceContext();
        else
            choiceContext ??= new HookPlayerChoiceContext(player, LocalContext.NetId.Value, GameActionType.Combat);

        await Figment.UseAndAdvanceMove(choiceContext, moveParams);
    }
}