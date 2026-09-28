using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Snacks;

public class FairyRing : SnackCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipHelper.ForImagine(this);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ImaginationCmd.ImagineRandom<ImaginaryFriendPower>(choiceContext, Owner);
        if (IsUpgraded) await ImaginationCmd.ImagineRandom<ImaginaryFriendPower>(choiceContext, Owner);
    }
}