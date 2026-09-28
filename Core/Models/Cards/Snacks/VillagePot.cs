using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Powers.Triggers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Snacks;

public class VillagePot : SnackCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        HoverTipHelper.ForImagineRandom<MahuizzotiaTrigger>(this);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ImaginationCmd.ImagineRandom<MahuizzotiaTrigger>(choiceContext, Owner);
        await CardPileCmd.AddGeneratedCardsToCombat(
            [Generate(Owner.Character.CardPool), Generate(ModelDb.CardPool<ColorlessCardPool>())],
            PileType.Hand, Owner);
    }

    private CardModel Generate(CardPoolModel cardPool)
    {
        var card = CardFactory.GetDistinctForCombat(
            Owner,
            cardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
                .Where(c => c.Type == CardType.Attack),
            1,
            Owner.RunState.Rng.CombatCardGeneration).First();
        if (IsUpgraded) card.AddKeyword(CardKeyword.Retain);
        return card;
    }
}