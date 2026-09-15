using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Models.Cards;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core.Hooks;

public static class TrimawHook
{
    public static async Task BeforeMorselPrepped(
        ICombatState combatState,
        PlayerChoiceContext choiceContext,
        Player player,
        Morsel morsel)
    {
        foreach (var hookListener in combatState.IterateHookListeners())
        {
            if (hookListener is not IPrepListener prepListener) continue;
            choiceContext.PushModel(hookListener);
            await prepListener.BeforeMorselPrepped(choiceContext, player, morsel);
            choiceContext.PopModel(hookListener);
            hookListener.InvokeExecutionFinished();
        }
    }

    public static async Task AfterMorselPrepped(
        ICombatState combatState,
        PlayerChoiceContext choiceContext,
        Player player,
        Morsel morsel,
        SnackCard? createdSnack)
    {
        foreach (var hookListener in combatState.IterateHookListeners())
        {
            if (hookListener is not IPrepListener prepListener) continue;
            choiceContext.PushModel(hookListener);
            await prepListener.AfterMorselPrepped(choiceContext, player, morsel, createdSnack);
            choiceContext.PopModel(hookListener);
            hookListener.InvokeExecutionFinished();
        }
    }
}