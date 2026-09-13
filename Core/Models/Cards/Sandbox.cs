using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.CardPools;
using Trimaw.Core.Commands;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards;

[Pool(typeof(ColorlessCardPool))]
public class Sandbox() : BaseTrimawCard(0, CardType.Skill, CardRarity.Basic, TargetType.Self, false)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipHelper.ForImagine(Owner),
        HoverTipFactory.Static(StaticHoverTip.Pop)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await ImaginationCmd.ImagineGachaPull(choiceContext, Owner);
    }

    protected override CardLocation GetResultLocationForCardPlay()
    {
        var location = base.GetResultLocationForCardPlay();
        return location.pileType != PileType.Discard
            ? location
            : new CardLocation(location.player, PileType.Hand, CardPilePosition.Bottom);
    }
}