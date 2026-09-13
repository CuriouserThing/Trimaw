using MegaCrit.Sts2.Core.Entities.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.SharedTalents;

public class AllEnergySpentTrigger : SingleCardPlayTrigger
{
    public override string Icon64Path => Pathfinder.NotoEmoji64("handshake_medium_light_skin_tone_light_skin_tone");
    public override string Icon256Path => Pathfinder.NotoEmoji256("handshake_medium_light_skin_tone_light_skin_tone");

    protected override bool CardMatches(CardPlay cardPlay)
    {
        // There can't be any energy left, and some energy had to be spent to get there
        // (So no 0-energy cards triggering it)
        return cardPlay.Player.PlayerCombatState?.Energy == 0 && cardPlay.Resources.EnergySpent >= 1;
    }
}