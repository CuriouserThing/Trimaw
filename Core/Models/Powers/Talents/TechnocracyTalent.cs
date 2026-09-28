using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Hooks;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Talents;

public class TechnocracyTalent : FigmentTalent, IFigmentListener
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("ophanim");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("ophanim");

    public bool ShouldPreventFigmentPop(Player player, Figment figment)
    {
        return Figment == figment;
    }

    public Task AfterPreventingFigmentPop(PlayerChoiceContext choiceContext, Player player, Figment figment)
    {
        Flash();
        return Task.CompletedTask;
    }

    public Task AfterFigmentImagined(PlayerChoiceContext choiceContext, Player player, Figment figment)
    {
        if (Figment == figment) return Task.CompletedTask;

        var hand = PileType.Hand.GetPile(player).Cards;
        if (hand.Count == 0) return Task.CompletedTask;

        var rng = player.RunState.Rng.CombatCardSelection;
        var card = rng.NextItem(hand.Where(c => CanonicallyNotFree(c) && CurrentlyNotFree(c)));
        card ??= rng.NextItem(hand.Where(CurrentlyNotFree));
        card ??= rng.NextItem(hand.Where(CanonicallyNotFree));
        card ??= rng.NextItem(hand);

        if (card is not null)
        {
            Flash();
            card.SetToFreeThisTurn();
        }

        return Task.CompletedTask;
    }

    private static bool CanonicallyNotFree(CardModel card)
    {
        return card.EnergyCost.GetWithModifiers(CostModifiers.None) > 0 || card.BaseStarCost > 0;
    }

    private static bool CurrentlyNotFree(CardModel card)
    {
        return card.CostsEnergyOrStars(true);
    }
}