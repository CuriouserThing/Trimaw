using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Models.Monsters;

namespace Trimaw.Core.Hooks;

public interface IFigmentListener
{
    bool ShouldPreventFigmentPop(Player player, Figment figment)
    {
        return false;
    }

    Task AfterPreventingFigmentPop(PlayerChoiceContext choiceContext, Player player, Figment figment)
    {
        return Task.CompletedTask;
    }

    Task BeforeFigmentImagined(PlayerChoiceContext choiceContext, Player player)
    {
        return Task.CompletedTask;
    }

    Task AfterFigmentImagined(PlayerChoiceContext choiceContext, Player player, Figment figment)
    {
        return Task.CompletedTask;
    }
}