using MegaCrit.Sts2.Core.Entities.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Triggers;

public class PowerTrigger : SingleCardPlayTrigger
{
    public override string Icon64Path => Pathfinder.NotoEmoji64("electric_light_bulb");
    public override string Icon256Path => Pathfinder.NotoEmoji256("electric_light_bulb");

    protected override bool CardMatches(CardPlay cardPlay)
    {
        return cardPlay.Card.Type == CardType.Power;
    }
}