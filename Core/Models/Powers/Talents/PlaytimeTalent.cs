using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Talents;

public class PlaytimeTalent : FigmentTalent
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.NotoEmoji64("game_die");
    public override string Icon256Path => Pathfinder.NotoEmoji256("game_die");

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Figment.PetOwner) return;

        if (CardPile.GetCards(player, PileType.Hand)
            .Any(c => c.EnergyCost.GetWithModifiers(CostModifiers.All) == 1)) return;

        var cards = CardPile.GetCards(player, PileType.Draw, PileType.Discard, PileType.Exhaust)
            .Where(c => c.EnergyCost.GetWithModifiers(CostModifiers.All) == 1);
        var card = player.RunState.Rng.CombatCardSelection.NextItem(cards);
        if (card is null) return;

        Flash();
        await CardPileCmd.Add(card, PileType.Hand);
    }
}