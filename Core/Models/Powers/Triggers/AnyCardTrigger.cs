using MegaCrit.Sts2.Core.Entities.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Triggers;

public class AnyCardTrigger : MultiCardPlayTrigger
{
    public override string Icon64Path => Pathfinder.NotoEmoji64("vertical_traffic_light");
    public override string Icon256Path => Pathfinder.NotoEmoji256("vertical_traffic_light");

    protected override bool CardMatches(CardPlay cardPlay)
    {
        return true;
    }
}