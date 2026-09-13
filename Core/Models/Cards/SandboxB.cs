using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.CardPools;
using Trimaw.Core.Commands;

namespace Trimaw.Core.Models.Cards;

[Pool(typeof(ColorlessCardPool))]
public class SandboxB() : BaseTrimawCard(1, CardType.Skill, CardRarity.Basic, TargetType.RandomEnemy, false)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Prep)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await SnackCmd.PrepRandom(choiceContext, CombatState, Owner);
    }

    protected override CardLocation GetResultLocationForCardPlay()
    {
        var location = base.GetResultLocationForCardPlay();
        return location.pileType != PileType.Discard
            ? location
            : new CardLocation(location.player, PileType.Hand, CardPilePosition.Bottom);
    }
}