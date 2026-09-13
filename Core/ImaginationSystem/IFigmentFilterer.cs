using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Trimaw.Core.ImaginationSystem;

public interface IFigmentFilterer
{
    TaggedFigment FilterFigment(Player owner, FigmentFilter filter);

    TaggedFigment GachaPullFigment(Player owner);

    Task ImagineFigment(PlayerChoiceContext choiceContext, Player owner, TaggedFigment figment);
}