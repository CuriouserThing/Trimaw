using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Trimaw.Core.Models.Cards.Snacks;

public class No1ShioRamen : SnackCard
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var count = CardPile.MaxCardsInHand - PileType.Hand.GetPile(Owner).Cards.Count;
        var cards = PileType.Draw.GetPile(Owner).Cards
            .Where(c => c.EnergyCost.GetWithModifiers(CostModifiers.All) == 1)
            .TakeRandom(count, Owner.RunState.Rng.CombatCardSelection)
            .ToArray();
        await CardPileCmd.Add(cards, PileType.Hand);
        if (IsUpgraded)
            foreach (var card in cards)
                card.SetToFreeThisTurn();
    }
}