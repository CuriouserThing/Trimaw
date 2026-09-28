using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class CryingThiefIntent : LabeledFigmentIntent<CryingThiefFigment>
{
    private static readonly CardsVar Cards = new(1);
    private static readonly CardsVar ExtraCards = new(nameof(ExtraCards), 1);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.CardDebuff;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("coat");

    private static int GetAmount(MoveContext ctx)
    {
        var payment = ctx.GetAmount();
        return (int)(Cards.BaseValue + payment * ExtraCards.BaseValue);
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<CryingThiefFigment> ctx)
    {
        label.Add(new CardsVar(GetAmount(ctx)));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<CryingThiefFigment> ctx)
    {
        desc.Add(Cards);
        desc.Add(ExtraCards);
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<CryingThiefFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        var cardCount = GetAmount(ctx);
        var hand = PileType.Hand.GetPile(ctx.PetOwner).Cards;
        cardCount = Math.Min(cardCount, hand.Count);

        var rng = ctx.PetOwner.RunState.Rng.CombatCardSelection;
        var cardsLeft = cardCount;
        foreach (var group in hand
                     .GroupBy(c => c.EnergyCost.GetWithModifiers(CostModifiers.All))
                     .OrderBy(g => g.Key))
        {
            var cards = group.ToList().StableShuffle(rng);
            for (var i = 0; i < cards.Count; i += 1)
            {
                if (cardsLeft == 0) break;
                cardsLeft -= 1;
                await CardCmd.Exhaust(choiceCtx, cards[i]);
            }

            if (cardsLeft == 0) break;
        }

        await CardPileCmd.Draw(choiceCtx, cardCount, ctx.PetOwner);
        return FigmentMoveResult.Success;
    }
}