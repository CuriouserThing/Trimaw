using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;

namespace Trimaw.Core.Models.Cards.Snacks;

public class BloodCurd : SnackCard
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = CardFactory.GetDistinctForCombat(
            Owner,
            ModelDb.Character<Ironclad>().CardPool
                .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint),
            CardPile.MaxCardsInHand - CardPile.GetCards(Owner, PileType.Hand).Count(),
            Owner.RunState.Rng.CombatCardGeneration).ToArray();

        foreach (var card in cards)
        {
            if (IsUpgraded) CardCmd.Upgrade(card);
            CardCmd.ApplyKeyword(card, CardKeyword.Ethereal);
        }

        await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, Owner);
    }
}