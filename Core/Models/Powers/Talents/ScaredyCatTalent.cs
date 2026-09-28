using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Talents;

public class ScaredyCatTalent : FigmentTalent
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("return_arrow");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("return_arrow");

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Figment.PetOwner) return;

        Flash();
        await Figment.Pop();
    }
}