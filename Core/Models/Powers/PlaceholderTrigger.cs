using MegaCrit.Sts2.Core.Entities.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers;

public class PlaceholderTrigger : SingleCardPlayTrigger
{
    public override string Icon64Path => Pathfinder.NotoEmoji64("construction");
    public override string Icon256Path => Pathfinder.NotoEmoji256("construction");

    protected override bool CardMatches(CardPlay cardPlay)
    {
        return true;
    }
}