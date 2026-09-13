using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Models.Cards;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core.Hooks;

public interface IPrepListener
{
    Task BeforeMorselPrepped(PlayerChoiceContext choiceContext, Player player, Morsel morsel)
    {
        return Task.CompletedTask;
    }

    Task AfterMorselPrepped(PlayerChoiceContext choiceContext, Player player, Morsel morsel, SnackCard? createdSnack)
    {
        return Task.CompletedTask;
    }
}