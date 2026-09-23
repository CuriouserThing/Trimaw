using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.Models.Cards.Snacks;

public class VillagePot : SnackCard
{
    private async Task Imagine(PlayerChoiceContext choiceContext)
    {
        await ImaginationCmd.ImagineRandom<ImaginaryShieldPower>(choiceContext, Owner);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await Imagine(choiceContext);
        if (IsUpgraded) await Imagine(choiceContext);
    }
}