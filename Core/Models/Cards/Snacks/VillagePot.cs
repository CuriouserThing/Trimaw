using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Commands;
using Trimaw.Core.ImaginationSystem;

namespace Trimaw.Core.Models.Cards.Snacks;

public class VillagePot : SnackCard
{
    private async Task Imagine(PlayerChoiceContext choiceContext)
    {
        await ImaginationCmd.Imagine(choiceContext, Owner, FigmentFilter.All.Requiring(HardTag.Attacking));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await Imagine(choiceContext);
        if (IsUpgraded) await Imagine(choiceContext);
    }
}