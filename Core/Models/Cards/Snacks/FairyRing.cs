using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Commands;

namespace Trimaw.Core.Models.Cards.Snacks;

public class FairyRing : SnackCard
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ImaginationCmd.ImagineGachaPull(choiceContext, Owner);
        if (IsUpgraded) await ImaginationCmd.ImagineGachaPull(choiceContext, Owner);
    }
}