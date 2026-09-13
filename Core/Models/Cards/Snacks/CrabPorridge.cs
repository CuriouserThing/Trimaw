using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Trimaw.Core.Models.Cards.Snacks;

public class CrabPorridge : SnackCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var rng = Owner.RunState.Rng.CombatCardSelection;
        var cardsToTake = Math.Min(DynamicVars.Cards.IntValue, Math.Min(
            PileType.Draw.GetPile(Owner).Cards.Count,
            CardPile.MaxCardsInHand - PileType.Hand.GetPile(Owner).Cards.Count));
        var cards = new CardModel[cardsToTake];

        foreach (var group in PileType.Draw.GetPile(Owner).Cards
                     .GroupBy(c => c.EnergyCost.GetWithModifiers(CostModifiers.Local))
                     .OrderByDescending(g => g.Key))
        {
            if (cardsToTake < 1) break;

            foreach (var card in group.TakeRandom(cardsToTake, rng))
            {
                cards[^cardsToTake] = card;
                cardsToTake -= 1;
            }
        }

        cards.TakeRandom(1, rng).FirstOrDefault()?.SetToFreeThisTurn();
        await CardPileCmd.Add(cards, PileType.Hand);
    }
}