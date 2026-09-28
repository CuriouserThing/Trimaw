using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Trimaw.Core.Models.Powers.Talents;

public class ScaredyCatTalent : FigmentTalent
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Figment.PetOwner) return;

        Flash();
        await Figment.Pop();
    }
}