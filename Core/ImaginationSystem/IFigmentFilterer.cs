using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.ImaginationSystem;

public interface IFigmentFilterer
{
    Figment? FilterFigment(Player owner, FigmentPower power);

    Task ImagineFigment(PlayerChoiceContext choiceContext, Player owner, Figment figment);
}